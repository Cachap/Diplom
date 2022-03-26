using UnityEngine;

namespace Assets.Scripts.Machine
{
    public class Tool : MonoBehaviour
    {
        public string Name { get; set; }

        public int Number { get; set; }

        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public Transform ShopToolTransform { get; set; }

        private GameObject ToolInPatronObject;
        private GameObject ToolObject;
  
        private Vector3 PositionPoint;

        public Tool()
        {
            ToolInPatronObject = Resources.Load<GameObject>("Patron");
        }

        public void AddToolInScene()
        {
            Hand.Patron_obj.Add(Instantiate(ToolInPatronObject, ShopToolTransform));
            Hand.Patron_obj[Number].transform.position = Position;
            Hand.Patron_obj[Number].transform.rotation = Rotation;

            PositionPoint = Hand.Patron_obj[Number].transform.GetChild(2).position;

            ToolObject = Hand.Patron_obj[Number].transform.GetChild(1).gameObject;

            Hand.ToolChange_obj.Add(ToolObject);
        }

        public void RotateTool()
        {
            Hand.Patron_obj[Number].transform.RotateAround(PositionPoint, Vector3.back, 90f);
        }
    }
}
