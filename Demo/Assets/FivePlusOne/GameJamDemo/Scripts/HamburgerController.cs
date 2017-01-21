
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FivePlusOne.GameJamDemo {
	/*
		main controller of the game
	*/

	public class HamburgerController : MonoBehaviour {

		[Tooltip("List of hamburger ingredients")]
		[SerializeField]
		List<IngredientObject> _ingredients;

		[Tooltip("The plate for target hamburger")]
		[SerializeField]
		Transform _targetBurgerPlate;

		[Tooltip("The plate for player hamburger")]
		[SerializeField]
		Transform _playerBurgerPlate;

		[Tooltip("Edit game difficulty")]
		[SerializeField]
		int _minLayer = 5;

		[Tooltip("Edit game difficulty")]
		[SerializeField]
		int _maxLayer = 10;

		[Tooltip("Background music")]
		[SerializeField]
		AudioSource _bgm;

		[Tooltip("SFX for next dish")]
		[SerializeField]
		AudioSource _nextDish;

		[Tooltip("SFX for next player layer")]
		[SerializeField]
		AudioSource _nextLayer;

		[Tooltip("SFX for winning a game")]
		[SerializeField]
		AudioSource _nextGame;

		// simple random number generator
		System.Random _randomNumber;

		// height of player and target burger
		float _playerLayerOffset;
		float _targetLayerOffset;

		// layers target burger
		List<HamburgerIngredient> _targetBurgerLayers;

		// current layer number for checking correctness
		int _playerLayerNumber;

		// next ingredient to add
		IngredientObject _nextIngredient;

		/*
			on script awake
		*/

		void Awake () {
			_randomNumber = new System.Random();
			_targetBurgerLayers = new List<HamburgerIngredient>();
		}

		/*
			on game start
		*/

		void Start () {
			_bgm.Play();
		}

		/*
			on game loop
		*/

		void Update () {
			HandlePlayerInput();
			AppendAndCheckLayer();
		}

		/*
			next target burger
		*/

		public void NextTargetBurger () {
			ClearPlayerBurger();
			ClearTargetBurger();
			CreateTargetBurger();
			_nextDish.Play();
		}

		/*
			create a target burger for reference
		*/

		void CreateTargetBurger () {
			_targetBurgerLayers = HamburgerGenerator.MakeBurger(_randomNumber.Next(_minLayer, _maxLayer));

			for (var i = 0; i < _targetBurgerLayers.Count; i++) {
				var ingredientObject = SearchIngredient(_targetBurgerLayers[i]);

				AddLayer(
					_targetBurgerPlate
					, ingredientObject.Ingredient
					, _targetLayerOffset
				);
				_targetLayerOffset += ingredientObject.Height;
			}
		}

		/*
			clean up target burger
		*/

		void ClearTargetBurger () {
			// remove all layers
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in _targetBurgerPlate) {
				children.Add(child.gameObject);
			}
			children.ForEach(child => {
				Destroy(child);
			});

			// reset layer height
			_targetLayerOffset = 0;
		}

		/*
			clean up player burger
		*/

		void ClearPlayerBurger () {
			// remove all layers
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in _playerBurgerPlate) {
				children.Add(child.gameObject);
			}
			children.ForEach(child => {
				Destroy(child);
			});

			// reset layer height
			_playerLayerOffset = 0;

			// reset layer number
			_playerLayerNumber = 0;
		}

		/*
			handle player hamburger making commands
		*/

		void HandlePlayerInput () {
			if (_targetBurgerLayers.Count == 0) {
				return;
			}

			// find ingredient from input
			if (Input.GetButtonUp("Pineapple")) {
				_nextIngredient = SearchIngredient(
					HamburgerIngredient.Pineapple
				);
			} else if (Input.GetButtonUp("Meat")) {
				_nextIngredient = SearchIngredient(
					HamburgerIngredient.Meat
				);
			} else if (Input.GetButtonUp("Lettuce")) {
				_nextIngredient = SearchIngredient(
					HamburgerIngredient.Lettuce
				);
			} else if (Input.GetButtonUp("Pickles")) {
				_nextIngredient = SearchIngredient(
					HamburgerIngredient.Pickles
				);
			} else if (Input.GetButtonUp("Bacon")) {
				_nextIngredient = SearchIngredient(
					HamburgerIngredient.Bacon
				);
			} else if (Input.GetButtonUp("Onions")) {
				_nextIngredient = SearchIngredient(
					HamburgerIngredient.Onions
				);
			} else if (Input.GetButtonUp("Cheese")) {
				_nextIngredient = SearchIngredient(
					HamburgerIngredient.Cheese
				);
			} else if (Input.GetButtonUp("Tomatoes")) {
				_nextIngredient = SearchIngredient(
					HamburgerIngredient.Tomatoes
				);
			} else if (Input.GetButtonUp("Bread")) {
				_nextIngredient = SearchIngredient(
					HamburgerIngredient.Bread
				);
			}

			if (_nextIngredient.Known) {
				PlayRandomSFX();
			}
		}

		void AppendAndCheckLayer () {
			if (_nextIngredient.Known) {
				// when exist, add layer to player burger
				AddLayer(
					_playerBurgerPlate
					, _nextIngredient.Ingredient
					, _playerLayerOffset
				);
				_playerLayerOffset += _nextIngredient.Height;

				// check correctness
				if (_nextIngredient.Name != _targetBurgerLayers[_playerLayerNumber]) {
					Debug.Log("Wrong input, update target burger.");
					NextTargetBurger();
				} else {
					_playerLayerNumber += 1;
				}

				// check win state
				if (_playerLayerNumber >= _targetBurgerLayers.Count) {
					Debug.Log("Hamburger done, update target burger.");
					NextTargetBurger();
					_nextGame.Play();
				}

				// reset next ingredient
				_nextIngredient = new IngredientObject(false);
			}
		}

		/*
			add a layer to a given burger
		*/

		void AddLayer (Transform burger, GameObject ingredient, float height) {
			// slight offset in x/z coordinate for flavor
			var offsetX = (float) _randomNumber.NextDouble() * 2 + 10f;
			var offsetZ = (float) _randomNumber.NextDouble() * 2 + 10f;
			var offsetY = 0f;

			if (burger == _targetBurgerPlate) {
				offsetY = height;
			} else {
				offsetY = height + 20f;
			}

			// create the layer at said location and said parent burger
			var layer = Instantiate(
				ingredient
				, new Vector3(offsetX, offsetY, offsetZ)
				, Quaternion.identity
			);
			layer.transform.SetParent(burger, false);
			layer.SetActive(true);

			if (burger == _playerBurgerPlate) {
				var rigidBody = layer.AddComponent<Rigidbody>();
				rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
			}
		}

		/*
			play random sound effect
		*/

		void PlayRandomSFX () {
			_nextLayer.Play();
		}

		/*
			search for an ingredient (should switch to key/value mapping)
		*/

		IngredientObject SearchIngredient (HamburgerIngredient name) {
			for (var i = 0; i < _ingredients.Count; i++) {
				if (_ingredients[i].Name == name) {
					return _ingredients[i];
				}
			}

			Debug.LogWarning(string.Format("Unknown ingredient \"{0}\", fallback to default ingredient.", name));
			return _ingredients[0];
		}
	}

	/*
		make ingredient easily editable in unity
	*/

	[System.Serializable]
	public struct IngredientObject {
		[SerializeField]
		HamburgerIngredient _name;

		[SerializeField]
		GameObject _ingredient;

		[SerializeField]
		float _height;

		[SerializeField]
		bool _known;

		public HamburgerIngredient Name {
			get { return _name; }
		}

		public GameObject Ingredient {
			get { return _ingredient; }
		}

		public float Height {
			get { return _height; }
		}

		public bool Known {
			get { return _known; }
		}

		public IngredientObject (bool known) {
			_name = HamburgerIngredient.Pineapple;
			_ingredient = new GameObject();
			_height = 0f;
			_known = known;
		}

		public IngredientObject (HamburgerIngredient name, GameObject ingredient, float height) {
			_name = name;
			_ingredient = ingredient;
			_height = height;
			_known = true;
		}
	}
}
