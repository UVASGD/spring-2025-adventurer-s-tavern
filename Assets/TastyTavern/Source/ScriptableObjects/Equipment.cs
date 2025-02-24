using UnityEngine;


[CreateAssetMenu(fileName = "Equiptment", menuName = "ScriptableObjects/Equiptment", order = 0)]
public class  Equipment : ScriptableObject 
{

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public string Description { get; set; }

    /// improve this structure

    [field: SerializeField]
    public Sprite[] Sprites { get; set; }  

    [field: SerializeField]
    public int Price { get; set; }
}
