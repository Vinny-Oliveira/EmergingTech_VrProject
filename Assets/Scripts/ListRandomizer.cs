using System.Collections.Generic;

/* Randomize the order of items in a list */
public class ListRandomizer {

    public static void Randomize<T>(ref List<T> listItems) {
        System.Random rand = new System.Random();

        // For each spot in the list, pick a random item to swap into that spot.
        for (int i = 0; i < listItems.Count - 1; i++) {
            int j = rand.Next(i, listItems.Count);
            T temp = listItems[i];
            listItems[i] = listItems[j];
            listItems[j] = temp;
        }
    }
}
