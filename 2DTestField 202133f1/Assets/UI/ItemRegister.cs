using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRegister : MonoBehaviour{
    [SerializeField] GameObject[] item_database = new GameObject[0];
    [SerializeField] GameObject[] image_database = new GameObject[0];
    [SerializeField] GameObject[] instance_database = new GameObject[0];
    public int[] item_price = new int[0];
    Dictionary<int , GameObject> item_id_to_image_dict = new Dictionary<int, GameObject>();
    Dictionary<int , GameObject> item_id_to_item_dict = new Dictionary<int, GameObject>();
    Dictionary<int , GameObject> item_id_to_instance = new Dictionary<int, GameObject>();
    private void Awake() {
        for(int i = 1 ; i <= item_database.Length ; i++){
            item_id_to_image_dict[i] = image_database[i - 1];
            item_id_to_item_dict[i] = item_database[i - 1];
            item_id_to_instance[i] = instance_database[i - 1];
        }
    }
    public GameObject item_id_to_image(int item_id){
        return item_id_to_image_dict[item_id];
    }
    public GameObject item_id_to_item(int item_id){
        return item_id_to_item_dict[item_id];
    }
    public GameObject item_id_to_instanced_item(int item_id){
        return item_id_to_instance[item_id];
    }
}
