using System;
using System.Windows.Forms;
using ReadWriteMemory;

namespace MWR_FoV_Changer
{
    public partial class Form1 : Form
    {
        ProcessMemory mwr;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoFoV();
        }

        ProcessMemory getProcessMemory(string processName)
        {
            if (string.IsNullOrEmpty(processName)) return null;
            if (processName.Contains(".exe")) processName = processName.Replace(".exe", string.Empty);
            var mem = new ProcessMemory(processName);
            if (mem != null && mem.CheckProcess())
            {
                mem.StartProcess();
                return mem;
            }
            return null;
        }


        int getIntPointerAddress(IntPtr baseAddress, int offset, ProcessMemory procMem)
        {
            if (procMem == null) return 0;
            var pointer = 0;
            var pointedTo = BitConverter.ToInt32(procMem.ReadMem(baseAddress.ToInt32(), 4), 0);
            pointer = IntPtr.Add((IntPtr)Convert.ToInt32(pointedTo.ToString("X"), 16), offset).ToInt32();
            return pointer;
        }

        void DoFoV()
        {
            try
            {
                if (mwr == null || !mwr.CheckProcess()) mwr = getProcessMemory("h1_sp64_ship");
                if (mwr == null)
                {
                    label3.Text = "Status: failed to find process";
                    return;
                }

                var fov = 65f;
                var vmFoV = fov;
                if (!float.TryParse(numericUpDown1.Value.ToString(), out fov)) return;
                if (!float.TryParse(numericUpDown3.Value.ToString(), out vmFoV)) return;

                var FoVAddr = 0x14C234A70;
                var vmFoVAddr = 0x14C234D70;

                var currentFoV = BitConverter.ToSingle(mwr.ReadMem(FoVAddr, 4), 0);
                var currentVMFoV = BitConverter.ToSingle(mwr.ReadMem(vmFoVAddr, 4), 0);
                if (checkBox1.Checked && currentFoV <= 64)
                {
                    label3.Text = "Status: not changing fov because cinematic mode";
                    return;
                }
                mwr.WriteFloat(FoVAddr, fov);
                mwr.WriteFloat(vmFoVAddr, vmFoV);
                label3.Text = "Status: Found process and wrote to memory";
                label2.Text = "Cur fov: " + BitConverter.ToSingle(mwr.ReadMem(FoVAddr, 4), 0) + " VM: " + BitConverter.ToSingle(mwr.ReadMem(vmFoVAddr, 4), 0);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to complete DoFoV(): " + Environment.NewLine + ex.ToString());
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) numericUpDown3.Value = numericUpDown1.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            DoFoV();
            if (checkBox2.Checked) numericUpDown3.Value = numericUpDown1.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DoFoV();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            DoFoV();
        }
    }
}
