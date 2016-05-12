using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PokemonController : MonoBehaviour {
	public Pokemon Pokemon;
	public PokemonController Enemy;
	public MessageController Messages;

	public Text PokemonName;
	public Text HP;
	public Image HPBar;
	public Image PokemonPicture;
	public Text[] Attacks;

	public int CurrentHP;
	public int MaxHP;

	public bool YourPokemon;

	AudioSource audioPlayer;
	// Use this for initialization
	void Start () {
		PokemonName.text = Pokemon.Name;

		CurrentHP = Pokemon.HP;
		MaxHP = Pokemon.HP;

		HP.text = GetCurrentHP();

		PokemonPicture.sprite = Pokemon.Picture;

		for (int i = 0; i < Attacks.Length; i++) {
			Attacks[i].text = Pokemon.Attacks[i].AttackName;
		}

		audioPlayer = gameObject.AddComponent<AudioSource>();
	}
	
	public string GetCurrentHP () {
		return CurrentHP + "/" + MaxHP;
	}

	public float GetHPFraction () {
		return (float) CurrentHP / (float) MaxHP;
	}

	public void Attack (int attackIndex) {

		Messages.AddMessage(AttackMessage(Pokemon.Attacks[attackIndex].AttackName));

		if (YourPokemon) {
			StartCoroutine(EnemyWaitToAttack(0.5f));
		}

		if (Pokemon.Attacks[attackIndex].AttackSound != null) {
			audioPlayer.clip = Pokemon.Attacks[attackIndex].AttackSound;
			audioPlayer.Play();
		}

		Enemy.Damage(Pokemon.Attacks[attackIndex].AttackPower);
	}

	public void ChooseAttack () {
		Attack(Random.Range(0, Pokemon.Attacks.Length));
	}

	public void Faint () {
		Messages.AddMessage(FaintedMessage());
		Messages.PokemonHasFainted = true;
	}

	public void Damage (int damage) {
		CurrentHP = CurrentHP - damage;

		if (CurrentHP < 0) {
			CurrentHP = 0;
		}

		HP.text = GetCurrentHP();
		HPBar.fillAmount = GetHPFraction();

		if (HasFainted()) {
			Faint();
		}
	}

	IEnumerator EnemyWaitToAttack (float waitTime) {
		yield return new WaitForSeconds(waitTime);

		if (!Messages.PokemonHasFainted) {
			Enemy.ChooseAttack();
		}
	}

	bool HasFainted () {
		return CurrentHP <= 0;
	}

	string AttackMessage (string attackName) {
		return Pokemon.Name + " used " + attackName;
	}

	string FaintedMessage () {
		return Pokemon.Name + " fainted!";
	}
}
