
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
			ClearTargetBurger();
			CreateTargetBurger();
		}

		/*
			create a target burger for reference
		*/

		void CreateTargetBurger () {
			_targetBurgerLayers = HamburgerGenerator.MakeBurger(_randomNumber.Next(5, 10));

			for (var i = 0; i < _targetBurgerLayers.Count; i++) {
				var ingredientObject = SearchIngredient(_targetBurgerLayers[i]);

				AddLayer(
					_targetBurgerPlate
					, ingredientObject.Ingredient
					, ingredientObject.Height
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
			add a layer to a given burger
		*/

		void AddLayer (Transform burger, GameObject ingredient, float height, float offsetY) {
			// slight offset in x/z coordinate for flavor
			var offsetX = (float) _randomNumber.NextDouble() / 10;
			var offsetZ = (float) _randomNumber.NextDouble() / 10;

			// create the layer at said location and said parent burger
			var layer = Instantiate(
				ingredient
				, new Vector3(offsetX, offsetY, offsetZ)
				, Quaternion.identity
			);
			layer.transform.SetParent(burger, false);
			layer.SetActive(true);
		}

		/*
			handle player hamburger making commands
		*/

		void HandlePlayerInput () {
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
			}
		}

		void AppendAndCheckLayer () {
			if (_nextIngredient.Known) {
				// when exist, add layer to player burger
				AddLayer(
					_playerBurgerPlate
					, _nextIngredient.Ingredient
					, _nextIngredient.Height
					, _playerLayerOffset
				);
				_playerLayerOffset += _nextIngredient.Height;

				// check correctness
				if (_nextIngredient.Name != _targetBurgerLayers[_playerLayerNumber]) {
					Debug.Log("Wrong input, update target burger.");
					ClearPlayerBurger();
					NextTargetBurger();
				} else {
					_playerLayerNumber += 1;
				}

				// reset next ingredient
				_nextIngredient = new IngredientObject(false);
			}
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
