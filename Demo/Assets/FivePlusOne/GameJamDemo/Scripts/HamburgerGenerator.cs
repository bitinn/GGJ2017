
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

			burger.Add(HamburgerIngredient.Bread);

			for (var i = 0; i < layerCount; i++) {
				// the number 9 allow us to randomly generate layers up to "bread"
				var ingredient = (HamburgerIngredient) ingredients.GetValue(randomNumber.Next(0, 9));
				burger.Add(ingredient);
			}

			burger.Add(HamburgerIngredient.Bread);

			return burger;
		}
	}

	/*
		known hamburger ingredients
	*/

	[System.Serializable]
	public enum HamburgerIngredient {
		Pineapple, Meat, Lettuce, Pickles, Bacon, Onions, Cheese, Tomatoes, Bread, Sword, Hotdog, Dice, Logo
	};
}
