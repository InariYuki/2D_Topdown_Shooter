using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadSystem : MonoBehaviour
{
    UI ui;
    [SerializeField] Stash player_stash;
    string path;
    FileStream save;
    private void Awake() {
        path = Application.persistentDataPath + "/Save.txt";
        ui = GetComponent<UI>();
    }
    public void Save(){
        save = new FileStream(path , FileMode.OpenOrCreate);
        StreamWriter file_writer = new StreamWriter(save);
        for(int i = 0; i < 26; i++){
            file_writer.WriteLine(ui.items_in_backpack[i]);
        }
        for(int i = 0; i < 20; i++){
            file_writer.WriteLine(player_stash.items_in_backpack[i]);
        }
        file_writer.WriteLine(ui.player_money);
        file_writer.Close();
        save.Close();
    }
    public void Load(){
        if(!File.Exists(path)) return;
        string[] load_data = File.ReadAllText(path).Split('\n');
        for(int i = 0; i < 26; i++){
            ui.items_in_backpack[i] = int.Parse(load_data[i]);
        }
        for(int i = 0; i < 20; i++){
            player_stash.items_in_backpack[i] = int.Parse(load_data[i + 26]);
        }
        ui.player_money = int.Parse(load_data[46]);
        if(ui.items_in_backpack[24] != 0){
            ItemData target_item = ui.Item_database.FindObject(ui.items_in_backpack[24]);
            Instantiate(target_item.item_instanced , ui.player.weapon.position , Quaternion.identity , ui.player.weapon);
        }
        if(ui.items_in_backpack[25] != 0){
            ItemData target_item = ui.Item_database.FindObject(ui.items_in_backpack[25]);
            Instantiate(target_item.item_instanced , ui.player.armor_holder.position , Quaternion.identity , ui.player.armor_holder);
        }
        ui.player.equip_armor();
        ui.player.equip_weapon();
        ui.RefreshMoneyDisplay();
    }
}
