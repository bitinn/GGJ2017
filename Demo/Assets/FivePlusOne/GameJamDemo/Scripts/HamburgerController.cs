
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
			List<HamburgerIngredient> targetBurgerLayers = HamburgerGenerator.MakeBurger(_randomNumber.Next(5, 10));

			for (var i = 0; i < targetBurgerLayers.Count; i++) {
				var ingredientObject = SearchIngredient(targetBurgerLayers[i]);

				AddLayer(
					_targetBurgerPlate
					, ingredientObject.Ingredient
					, ingredientObject.Height
				);
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
			add a layer to a given burger
		*/

		void AddLayer (Transform burger, GameObject ingredient, float height) {
			// slight offset in x/z coordinate for flavor
			var offsetX = (float) _randomNumber.NextDouble() / 10;
			var offsetZ = (float) _randomNumber.NextDouble() / 10;

			// create the layer at said location and said parent burger
			var layer = Instantiate(
				ingredient
				, new Vector3(offsetX, _targetLayerOffset, offsetZ)
				, Quaternion.identity
			);
			layer.transform.SetParent(burger, false);
			layer.SetActive(true);

			// offset for the next layer
			_targetLayerOffset += height;
		}

		/*
			handle player hamburger making commands
		*/

		void HandlePlayerInput () {
			if (Input.GetButtonUp("AnyIngredient")) {
				// TODO
				_playerLayerOffset += 0.2f;
			}
		}

		/*
			search for an ingredient
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

		public HamburgerIngredient Name {
			get { return _name; }
		}

		public GameObject Ingredient {
			get { return _ingredient; }
		}

		public float Height {
			get { return _height; }
		}

		public IngredientObject (HamburgerIngredient name, GameObject ingredient, float height) {
			_name = name;
			_ingredient = ingredient;
			_height = height;
		}
	}
}
