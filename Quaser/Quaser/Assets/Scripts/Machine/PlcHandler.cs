using AxiOMADataTest;

namespace Assets.Scripts.Machine
{
    public class PlcHandler
    {
        public enum HandInputStates { RotateAngle_90, Down, RotateAngle_180, Up, Return, PneumaticCylinder, None }
        public enum HandOutputStates { RotateAngle_90, Down, RotateAngle_180, Up, Return, PneumaticCylinder, None }
        public enum ShopToolStates { CwRotation, CcwRotation, None }

        public HandInputStates handInputState;
        public HandOutputStates handOutupState;
        public ShopToolStates shopToolState;

        private int numberBitHand;
        private int numberBitShopTool;

        public int numberTool = 0;

        public void ReadPlcRotate()
        {
            numberBitShopTool = 3;
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
                    handInputState = HandInputStates.PneumaticCylinder;
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
                    handOutupState = HandOutputStates.RotateAngle_90;
                    break;

                case 1:
                    handOutupState = HandOutputStates.Down;
                    break;

                case 2:
                    handOutupState = HandOutputStates.RotateAngle_180;
                    break;

                case 3:
                    handOutupState = HandOutputStates.Up;
                    break;

                case 4:
                    handOutupState = HandOutputStates.Return;
                    break;

                case 5:
                    handOutupState = HandOutputStates.PneumaticCylinder;
                    break;

                case 6:
                    handOutupState = HandOutputStates.None;
                    break;
            }
        }

        public void WritePlc()
        {
            for(int i = 0; i < Form1.inputValue.Length; i++)
                Form1.inputValue[i] = false;

            switch (handInputState)
            {
                case HandInputStates.PneumaticCylinder:
                    Form1.inputValue[0] = true;
                    break;

                case HandInputStates.RotateAngle_90:
                    Form1.inputValue[1] = true;
                    break;

                case HandInputStates.Down:
                    Form1.inputValue[2] = true;
                    break;

                case HandInputStates.RotateAngle_180:
                    Form1.inputValue[3] = true;
                    break;

                case HandInputStates.Up:
                    Form1.inputValue[4] = true;
                    break;

                case HandInputStates.Return:
                    Form1.inputValue[5] = true;
                    break;

                case HandInputStates.None:
                    break;
            }
        }
    }
}

