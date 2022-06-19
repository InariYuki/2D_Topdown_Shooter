using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRegister : MonoBehaviour{
    public GameObject thermo_katana;
    public GameObject thermo_katana_item;
    public Image thermo_katana_image;
    Dictionary<int , Image> item_id_to_image_dict = new Dictionary<int, Image>();
    private void Awake() {
        item_id_to_image_dict[1] = thermo_katana_image;
    }
    public Image item_id_to_image(int item_id){
        return item_id_to_image_dict[item_id];
    }
}
