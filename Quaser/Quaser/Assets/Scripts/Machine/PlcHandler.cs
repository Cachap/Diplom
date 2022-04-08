using AxiOMADataTest;

namespace Assets.Scripts.Machine
{
    public class PlcHandler
    {
        public bool[] inputData = new bool[8];
        public bool[] outputData = new bool[8];

        public bool[] plcRotate = Form1.plcRotate;

        public int numberTool = 12;

        public void ReadIOPlc()
        {
            for(int i = 0; i < inputData.Length; i++)
            {
                if(inputData[i] != Form1.inputValue[i])
                {
                    inputData[i] = Form1.inputValue[i];
                }
            }
            
            for (int i = 0; i < outputData.Length; i++)
            {
                if (outputData[i] != Form1.outputValue[i])
                {
                    outputData[i] = Form1.outputValue[i];
                }
            }
        }

        public void WriteIOPlc()
        {
            for (int i = 0; i < inputData.Length; i++)
            {
                if (Form1.inputValue[i] != inputData[i])
                {
                    Form1.inputValue[i] = inputData[i];
                }
            }

            for (int i = 0; i < outputData.Length; i++)
            {
                if (Form1.outputValue[i] != outputData[i])
                {
                    Form1.outputValue[i] = outputData[i];
                }
            }
        }
    }
}

