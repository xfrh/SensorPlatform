using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace Continuous
{
    class OctivInterface
    {
        const string DLL_PATH = @"octiv2.dll";

        public struct octivConfig
        {
            public IntPtr reportRate;
            public IntPtr triggerHoldoff;
            public IntPtr triggerDelay;
            public IntPtr bUseExternalTrigger;
            public IntPtr frequencyCount;
            public IntPtr availableHarmonicsArray;
            public IntPtr activatedHarmonicsArray;
        };

        // 3.01 Open
        [DllImport(DLL_PATH, EntryPoint = "octivOpen")]
        public static extern int octivOpen(
            [MarshalAs(UnmanagedType.LPStr)] string str_SerialNumber);

        // 3.02 Close
        [DllImport(DLL_PATH, EntryPoint = "octivClose")]
        public static extern int octivClose(
            int hDev);

        // 3.03 Get Firmware Revision
        [DllImport(DLL_PATH, EntryPoint = "octivGetFirmwareRevision")]
        public static extern int octivGetFirmwareRevision(
            int hDev,
            StringBuilder str_SerialNumber); // buffer: 128 bytes

        // 3.04 Get Serial Number
        [DllImport(DLL_PATH, EntryPoint = "octivGetSerial")]
        public static extern int octivGetSerial(
            int hDev,
            StringBuilder str_SerialNumber); // buffer: 32 bytes

        // 3.05 Get Model
        [DllImport(DLL_PATH, EntryPoint = "octivGetModel")]
        public static extern int octivGetModel(
            int hDev);

        // 3.06 Get Frequency Count
        [DllImport(DLL_PATH, EntryPoint = "octivGetFrequencyCount")]
        public static extern int octivGetFrequencyCount(
            int hDev);

        // 3.07 Get Frequency Details
        [DllImport(DLL_PATH, EntryPoint = "octivGetFrequencyDetails")]
        public static extern int octivGetFrequencyDetails(
            int hDev,
            int freqIndex,
            ref double pFreqVal,
            ref int pHarmCount);

        // 3.08 Set Mono Frequency
        [DllImport(DLL_PATH, EntryPoint = "octivSetMonoFrequency")]
        public static extern int octivSetMonoFrequency(
            int hDev,
            int freq);

        // 3.09 Set VI Frequency
        [DllImport(DLL_PATH, EntryPoint = "octivSetViFrequency")]
        public static extern int octivSetViFrequency(
            int hDev,
            int fIndex,
            int nHarm);

        // 3.10 Configure Fundamental Frequencies 
        [DllImport(DLL_PATH, EntryPoint = "octivConfigureFundamentalFrequencies")]
        public static extern int octivConfigureFundamentalFrequencies(
            int hDev,
            IntPtr vHarmonics,
            int vHarmSize);

        // 3.11 Set Report Rate
        [DllImport(DLL_PATH, EntryPoint = "octivSetReportRate")]
        public static extern int octivSetReportRate(
            int hDev,
            int nRate);

        // 3.12.1 Get Octiv Monno Sensor Values
        [DllImport(DLL_PATH, EntryPoint = "octivGetMonoSensorValues")]
        public static extern int octivGetMonoSensorValues(
            int hDev,
            ref int pTimeDiff,
            ref double pPower,
            ref double pForPower,
            ref double pSWR,
            ref double pRefPower,
            ref double pZR,
            ref double pZI,
            ref int pNTries);

        // 3.12.2 Get Octiv Monno Sensor Values
        [DllImport(DLL_PATH, EntryPoint = "octivReadMonoSensorValues")]
        public static extern int octivReadMonoSensorValues(
            int hDev,
            ref int pTimeDiff,
            ref double pPower,
            ref double pForPower,
            ref double pSWR,
            ref double pRefPower,
            ref double pZR,
            ref double pZI);

        // 3.13.1 Get VI Sensor Values
        [DllImport(DLL_PATH, EntryPoint = "octivGetViSensorValues")]
        public static extern int octivGetVISensorValues(
            int hDev,
            ref int pTimeDiff,
            IntPtr pV,
            IntPtr pI,
            IntPtr pP,
            ref double pPeak,
            ref int pDelay,
            ref int pnHarmonics,
            ref int pnTries);

        // 3.13.2 read VI Sensor Values
        [DllImport(DLL_PATH, EntryPoint = "octivReadVISensorValues")]
        public static extern int octivReadVISensorValues(
            int hDev,
            ref int pTimeDiff,
            IntPtr pV,
            IntPtr pI,
            IntPtr pP,
            ref double pPeak,
            ref int pDelay,
            ref int pnHarmonics);


        // 3.14.1 Get Sensor Values
        [DllImport(DLL_PATH, EntryPoint = "octivGetSensorValues")]
        public static extern int octivGetSensorValues(
            int hDev,
            IntPtr pV,
            IntPtr pI,
            IntPtr pP,
            IntPtr pPV,
            ref double pPeak,
            ref double pDelay,
            ref int pnValues,
            ref int pnTries);

        // 3.14.2 Read Sensor Data
        [DllImport(DLL_PATH, EntryPoint = "octivReadSensorData")]
        public static extern int octivReadSensorData(
            int hDev,
            IntPtr pV,
            IntPtr pI,
            IntPtr pP,
            IntPtr pPV,
            ref double pPeak,
            ref double pDelay,
            ref int pnValues,
            ref double DC);

        // 3.15.1 Get Sensor Data
        [DllImport(DLL_PATH, EntryPoint = "octivGetData")]
        public static extern int octivGetData(
            int hDev,
            IntPtr pData,
            ref int pnValues,
            ref int pTimeDiff,
            ref double pPeak,
            ref int pDelay,
            ref int pnTries);

        // 3.15.2 Read Data
        [DllImport(DLL_PATH, EntryPoint = "octivReadData")]
        public static extern int octivReadData(
            int hDev,
            IntPtr pData,
            ref int pnValues,
            ref int pTimeDiff,
            ref double pPeak,
            ref double pDelay);


        // 3.16 Get Parameter Name
        [DllImport(DLL_PATH, EntryPoint = "octivGetParameterName")]
        public static extern int octivGetParameterName(
            int hDev,
            int parameter,
            StringBuilder pParamName, // buffer: 10 bytes
            StringBuilder pParamDescription); // buffer: 30 bytes

        // 3.17 Use External Trigger
        [DllImport(DLL_PATH, EntryPoint = "octivUseExternalTrigger")]
        public static extern int octivUseExternalTrigger(
            int hDev,
            int bUseTrigger);

        // 3.18 Set Trigger Delay
        [DllImport(DLL_PATH, EntryPoint = "octivSetTriggerDelay")]
        public static extern int octivSetTriggerDelay(
            int hDev,
            int value);

        // 3.19 Get Sensor Configuration
        [DllImport(DLL_PATH, EntryPoint = "octivGetSensorConfiguration")]
        public static extern int octivGetSensorConfiguration(
            int hDev,
            octivConfig sConf);

        // 3.20 Set Trigger Holdoff
        [DllImport(DLL_PATH, EntryPoint = "octivSetTriggerHoldoff")]
        public static extern int octivSetTriggerHoldoff(
            int nDev,
            int value);


        // 3.21 Change Channel Lock
        [DllImport(DLL_PATH, EntryPoint = "octivLockOnChannel2")]
        public static extern int octivSetChannelLock(
            int nDev,
            int channel);


        // 3.26 Set Ion Flux Info
        [DllImport(DLL_PATH, EntryPoint = "octivSetIonFluxInfo")]
        public static extern int octivSetIonFluxInfo(
            int nDev,
            bool enable,
            double voltageDrop,
            double seriesResistance,
            double electrodeArea);

        // 3.27 Get Ion Flux Info
        [DllImport(DLL_PATH, EntryPoint = "octivGetIonFluxInfo")]
        public static extern int octivGetIonFluxInfo(
            int nDev,
            ref bool enabled,
            ref double voltageDrop,
            ref double seriesResistance,
            ref double electrodeArea);

        // 3.28 Set Number of Accumulations
        [DllImport(DLL_PATH, EntryPoint = "octivSetNumberOfAccumulations")]
        public static extern int octivSetNumberOfAccumulations(
            int nDev,
            int averages);

        // 3.29 Set Trigger Value
        [DllImport(DLL_PATH, EntryPoint = "octivSetTriggerValue")]
        public static extern int octivSetTriggerValue(
            int nDev,
            int level);

        // 3.30 Get Delay Resolution
        [DllImport(DLL_PATH, EntryPoint = "octivGetDelayResolution")]
        public static extern double octivGetDelayResolution(int nDev);
    }
}
