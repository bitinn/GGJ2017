
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
			float offsetY = 0;
			List<HamburgerIngredient> targetBurgerLayers = HamburgerGenerator.MakeBurger(6);

			for (var i = 0; i < targetBurgerLayers.Count; i++) {
				var ingredientObject = SearchIngredient(targetBurgerLayers[i]);

				var layer = Instantiate(
					ingredientObject.Ingredient
					, new Vector3(0, offsetY, 0)
					, Quaternion.identity
					, _targetBurgerPlate
				);
				layer.SetActive(true);

				offsetY += ingredientObject.Height;
			}
		}

		/*
			clean up target burger
		*/

		void ClearTargetBurger () {
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in _targetBurgerPlate) {
				children.Add(child.gameObject);
			}
			children.ForEach(child => {
				Destroy(child);
			});
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
