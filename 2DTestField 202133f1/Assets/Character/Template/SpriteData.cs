using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable/SpriteDatabase" , fileName ="SpriteDatabase")]
public class SpriteData : ScriptableObject
{
    public List<Head> head_database = new List<Head>();
    public List<Body> body_database = new List<Body>();
    public List<Limb> limb_database = new List<Limb>();
}
[System.Serializable] public class Head{
    public Sprite head_s , head_f , head_b;
}
[System.Serializable] public class Body{
    public Sprite body_s , body_f , body_b;
}
[System.Serializable] public class Limb{
    public Sprite right_hand_s , left_hand_s , right_leg_s , left_leg_s , right_hand_v , left_hand_v , right_leg_v , left_leg_v;
}
