using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMap : MonoBehaviour
{
    [SerializeField] GameObject bar_sector , store_sector;
    [SerializeField] List<GameObject> city_sector = new List<GameObject>();
    public void generate_city_sector(int column , int row){
        List<Vector2> map_coordinate = generate_map_coordinate(column , row);
        instance_desinated_sector(bar_sector , map_coordinate);
        instance_desinated_sector(store_sector , map_coordinate);
        while(map_coordinate.Count > 0){
            instance_desinated_sector(city_sector[Random.Range(0 , city_sector.Count)] , map_coordinate);
        }
    }
    void instance_desinated_sector(GameObject sector , List<Vector2> map_coordinate){
        int random_position = Random.Range(0 , map_coordinate.Count);
        Instantiate(sector , map_coordinate[random_position] , Quaternion.identity , transform);
        map_coordinate.Remove(map_coordinate[random_position]);
    }
    List<Vector2> generate_map_coordinate(int column , int row){
        List<Vector2> map_coordinate = new List<Vector2>();
        for(int i = 0; i < column; i++){
            for(int j = 0; j < row; j++){
                map_coordinate.Add(new Vector2(i , j) * 6.4f);
            }
        }
        return map_coordinate;
    }
    public void RemoveCitySector(){
        for(int i = 4 ; i < transform.childCount; i++){
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
