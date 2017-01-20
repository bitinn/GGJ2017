
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FivePlusOne.GameJamDemo {
	/*
		generate a sensible target hamburger for players to achieve
	*/

	public static class HamburgerGenerator {
		/*
			generate a list of ingredients given number of layers
		*/

		public static List<HamburgerIngredient> MakeBurger (int layerCount = 3) {
			var burger = new List<HamburgerIngredient>();
			var ingredients = System.Enum.GetValues(typeof(HamburgerIngredient));
			var randomNumber = new System.Random();

			for (var i = 0; i < layerCount; i++) {
				var ingredient = (HamburgerIngredient) ingredients.GetValue(randomNumber.Next(0, ingredients.Length));
				burger.Add(ingredient);
			}

			return burger;
		}
	}

	/*
		known hamburger ingredients
	*/

	[System.Serializable]
	public enum HamburgerIngredient {
		Pineapple, Meat, Lettuce, Pickles, Bacon, Onions, Cheese, Tomatoes
	};
}
