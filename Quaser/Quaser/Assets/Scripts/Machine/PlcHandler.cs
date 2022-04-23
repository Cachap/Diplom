using AxiOMADataTest;

namespace Assets.Scripts.Machine
{
    public class PlcHandler
    {
        public enum HandInputStates { C1, _90_tool, Lock_tool, Hand_D, _180an, Hand_U, _90_Def, None}
        public enum HandOutputStates {_90_tool, Lock_tool, Hand_down, Hand_up, _180an, _90_default, None }
        public enum ShopToolStates { CwRotation, CcwRotation, C1, None }

        public HandInputStates handInputState;
        public HandOutputStates handOutupState;
        public ShopToolStates shopToolState;

        private int numberBitHand;
        private int numberBitShopTool;

        public byte numberCurrentTool = 0;
        public int numberTool = 100;

        public void ReadPlcRotate()
        {
            numberBitShopTool = 3;
            numberCurrentTool = Form1.currentluNumberTool;
            numberTool = Form1.numberTool;

            for (int i = 0; i < Form1.plcRotate.Length; i++)
            {
                if(Form1.plcRotate[i] == true)
                    numberBitShopTool = i;
            }

            switch (numberBitShopTool)
            {
                case 0:
                    shopToolState = ShopToolStates.CwRotation;
                    break;

                case 1:
                    shopToolState = ShopToolStates.CcwRotation;
                    break;

                case 2:
                    shopToolState = ShopToolStates.C1;
                    break;

                case 3:
                    shopToolState = ShopToolStates.None;
                    break;
            }
        }

        public void ReadPlc()
        {
            numberBitHand = 6;

            for (int i = 0; i < Form1.outputValue.Length; i++)
            {
                if (Form1.outputValue[i] == true)
                {
                    numberBitHand = i;
                }
            }

            switch (numberBitHand)
            {
                case 0:
                    handOutupState = HandOutputStates._90_tool;
                    break;

                case 1:
                    handOutupState = HandOutputStates.Lock_tool;
                    break;

                case 2:
                    handOutupState = HandOutputStates.Hand_down;
                    break;

                case 3:
                    handOutupState = HandOutputStates.Hand_up;
                    break;

                case 4:
                    handOutupState = HandOutputStates._180an;
                    break;

                case 5:
                    handOutupState = HandOutputStates._90_default;
                    break;

                case 6:
                    handOutupState = HandOutputStates.None;
                    break;
            }
        }

        //Двигатель
        public void Impulse(bool value)
        {
            Form1.impuls = value;
        }

        public void WritePlc()
        {
            //for(int i = 0; i < Form1.inputValue.Length; i++)
            //    Form1.inputValue[i] = false;

            switch (handInputState)
            {
                case HandInputStates.C1:
                    Form1.inputValue[0] = true;
                    break;

                case HandInputStates._90_tool:
                    Form1.inputValue[1] = true;
                    break;

                case HandInputStates.Lock_tool:
                    Form1.inputValue[2] = true;
                    break;

                case HandInputStates.Hand_D:
                    Form1.inputValue[3] = true;
                    break;

                case HandInputStates._180an:
                    Form1.inputValue[4] = true;
                    break;

                case HandInputStates.Hand_U:
                    Form1.inputValue[5] = true;
                    break;

                case HandInputStates._90_Def:
                    Form1.inputValue[6] = true;
					break;

                case HandInputStates.None:
                {
                    for (int i = 0; i < Form1.inputValue.Length; i++)
                        Form1.inputValue[i] = false;
                        break;
                }
            }
        }
    }
}

