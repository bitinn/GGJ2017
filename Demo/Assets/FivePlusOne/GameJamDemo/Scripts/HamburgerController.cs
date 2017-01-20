
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
		List<IngredientObject> ingredients;

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

		public HamburgerIngredient Name {
			get { return _name; }
		}

		public GameObject Ingredient {
			get { return _ingredient; }
		}

		public IngredientObject (HamburgerIngredient name, GameObject ingredient) {
			_name = name;
			_ingredient = ingredient;
		}
	}
}
