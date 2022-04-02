using UnityEngine;

namespace Assets.Scripts.Machine
{
    public class Tool : MonoBehaviour
    {
        public string Name { get; set; }

        public int Number { get; set; }

        public int Length { get; set; }

        public int Radius { get; set; }

        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public Transform ParentTransform { get; set; }

        private readonly GameObject ToolInPatronObject;

        private readonly GameObject ToolObjectLoad;

        public GameObject ToolObject;
        
        //Позиция оси вращения 
        private Vector3 PositionPoint;

        public Tool()
        {
            ToolInPatronObject = Resources.Load<GameObject>("Patron");
            ToolObjectLoad = Resources.Load<GameObject>("Цанговый патрон");

            Length = 200;
            Radius = 50;
        }

        public void AddToolInShopTool()
        {
            for (int i = 0; i < Hand.ChangeTools.Count; i++)
            {
                if (Number == Hand.ChangeTools[i].Number)
                {
                    Destroy(Hand.PatronObjects[i]);
                    Hand.PatronObjects.RemoveAt(i);
                    Hand.ChangeTools.RemoveAt(i);
                }
            }

            Hand.PatronObjects.Add(Instantiate(ToolInPatronObject, ParentTransform));
            Hand.PatronObjects[Number].transform.position = Position;
            Hand.PatronObjects[Number].transform.rotation = Rotation;

            PositionPoint = Hand.PatronObjects[Number].transform.GetChild(2).position;

            ToolObject = Hand.PatronObjects[Number].transform.GetChild(1).gameObject;

            Hand.ChangeTools.Add(this);
        }

        public void AddToolInSpindle()
        {
            ToolObject = Instantiate(ToolObjectLoad, ParentTransform);
            ToolObject.transform.position = Position;
            ToolObject.transform.rotation = Rotation;
            ToolObject.tag = "CurrentTool";

            Hand.CurrentTool = this;

            TextUpdate.Change(this);
        }

        public void RotateToolToCapture()
        {
            Hand.PatronObjects[Number].transform.RotateAround(PositionPoint, Vector3.back, 90f);
        }

        public void ReturnTool()
        {
            Hand.PatronObjects[Number].transform.RotateAround(PositionPoint, Vector3.back, -90f);
        }
    }
}
