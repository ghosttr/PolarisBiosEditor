using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PolarisB
{
    public static class BiosStruct
    {
        public static int atom_rom_checksum_offset = 0x21;
        public static int ATOM_ROM_HEADER_ptr = 0x48;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_COMMON_TABLE_HEADER
        {
            Int16 usStructureSize;
            Byte ucTableFormatRevision;
            Byte ucTableContentRevision;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_ROM_HEADER
        {
            public ATOM_COMMON_TABLE_HEADER sHeader;
            public UInt32 uaFirmWareSignature;
            public UInt16 usBiosRuntimeSegmentAddress;
            public UInt16 usProtectedModeInfoOffset;
            public UInt16 usConfigFilenameOffset;
            public UInt16 usCRC_BlockOffset;
            public UInt16 usBIOS_BootupMessageOffset;
            public UInt16 usInt10Offset;
            public UInt16 usPciBusDevInitCode;
            public UInt16 usIoBaseAddress;
            public UInt16 usSubsystemVendorID;
            public UInt16 usSubsystemID;
            public UInt16 usPCI_InfoOffset;
            public UInt16 usMasterCommandTableOffset;
            public UInt16 usMasterDataTableOffset;
            public Byte ucExtendedFunctionCode;
            public Byte ucReserved;
            public UInt32 ulPSPDirTableOffset;
            public UInt16 usVendorID;
            public UInt16 usDeviceID;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_DATA_TABLES
        {
            public ATOM_COMMON_TABLE_HEADER sHeader;
            public UInt16 UtilityPipeLine;
            public UInt16 MultimediaCapabilityInfo;
            public UInt16 MultimediaConfigInfo;
            public UInt16 StandardVESA_Timing;
            public UInt16 FirmwareInfo;
            public UInt16 PaletteData;
            public UInt16 LCD_Info;
            public UInt16 DIGTransmitterInfo;
            public UInt16 SMU_Info;
            public UInt16 SupportedDevicesInfo;
            public UInt16 GPIO_I2C_Info;
            public UInt16 VRAM_UsageByFirmware;
            public UInt16 GPIO_Pin_LUT;
            public UInt16 VESA_ToInternalModeLUT;
            public UInt16 GFX_Info;
            public UInt16 PowerPlayInfo;
            public UInt16 GPUVirtualizationInfo;
            public UInt16 SaveRestoreInfo;
            public UInt16 PPLL_SS_Info;
            public UInt16 OemInfo;
            public UInt16 XTMDS_Info;
            public UInt16 MclkSS_Info;
            public UInt16 Object_Header;
            public UInt16 IndirectIOAccess;
            public UInt16 MC_InitParameter;
            public UInt16 ASIC_VDDC_Info;
            public UInt16 ASIC_InternalSS_Info;
            public UInt16 TV_VideoMode;
            public UInt16 VRAM_Info;
            public UInt16 MemoryTrainingInfo;
            public UInt16 IntegratedSystemInfo;
            public UInt16 ASIC_ProfilingInfo;
            public UInt16 VoltageObjectInfo;
            public UInt16 PowerSourceInfo;
            public UInt16 ServiceInfo;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_POWERPLAY_TABLE
        {
            public ATOM_COMMON_TABLE_HEADER sHeader;
            public Byte ucTableRevision;
            public UInt16 usTableSize;
            public UInt32 ulGoldenPPID;
            public UInt32 ulGoldenRevision;
            public UInt16 usFormatID;
            public UInt16 usVoltageTime;
            public UInt32 ulPlatformCaps;
            public UInt32 ulMaxODEngineClock;
            public UInt32 ulMaxODMemoryClock;
            public UInt16 usPowerControlLimit;
            public UInt16 usUlvVoltageOffset;
            public UInt16 usStateArrayOffset;
            public UInt16 usFanTableOffset;
            public UInt16 usThermalControllerOffset;
            public UInt16 usReserv;
            public UInt16 usMclkDependencyTableOffset;
            public UInt16 usSclkDependencyTableOffset;
            public UInt16 usVddcLookupTableOffset;
            public UInt16 usVddgfxLookupTableOffset;
            public UInt16 usMMDependencyTableOffset;
            public UInt16 usVCEStateTableOffset;
            public UInt16 usPPMTableOffset;
            public UInt16 usPowerTuneTableOffset;
            public UInt16 usHardLimitTableOffset;
            public UInt16 usPCIETableOffset;
            public UInt16 usGPIOTableOffset;
            public fixed UInt16 usReserved[6];
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_MCLK_ENTRY
        {
            public Byte ucVddcInd;
            public UInt16 usVddci;
            public UInt16 usVddgfxOffset;
            public UInt16 usMvdd;
            public UInt32 ulMclk;
            public UInt16 usReserved;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_MCLK_TABLE
        {
            public Byte ucRevId;
            public Byte ucNumEntries;
            // public ATOM_MCLK_ENTRY entries[ucNumEntries];
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_SCLK_ENTRY
        {
            public Byte ucVddInd;
            public UInt16 usVddcOffset;
            public UInt32 ulSclk;
            public UInt16 usEdcCurrent;
            public Byte ucReliabilityTemperature;
            public Byte ucCKSVOffsetandDisable;
            public UInt32 ulSclkOffset; // Polaris Only, remove for compatibility with Fiji
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_SCLK_TABLE
        {
            public Byte ucRevId;
            public Byte ucNumEntries;
            // public ATOM_SCLK_ENTRY entries[ucNumEntries];
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_VOLTAGE_ENTRY
        {
            public UInt16 usVdd;
            public UInt16 usCACLow;
            public UInt16 usCACMid;
            public UInt16 usCACHigh;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_VOLTAGE_TABLE
        {
            public Byte ucRevId;
            public Byte ucNumEntries;
            // public ATOM_VOLTAGE_ENTRY entries[ucNumEntries];
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_FAN_TABLE
        {
            public Byte ucRevId;
            public Byte ucTHyst;
            public UInt16 usTMin;
            public UInt16 usTMed;
            public UInt16 usTHigh;
            public UInt16 usPWMMin;
            public UInt16 usPWMMed;
            public UInt16 usPWMHigh;
            public UInt16 usTMax;
            public Byte ucFanControlMode;
            public UInt16 usFanPWMMax;
            public UInt16 usFanOutputSensitivity;
            public UInt16 usFanRPMMax;
            public UInt32 ulMinFanSCLKAcousticLimit;
            public Byte ucTargetTemperature;
            public Byte ucMinimumPWMLimit;
            public UInt16 usFanGainEdge;
            public UInt16 usFanGainHotspot;
            public UInt16 usFanGainLiquid;
            public UInt16 usFanGainVrVddc;
            public UInt16 usFanGainVrMvdd;
            public UInt16 usFanGainPlx;
            public UInt16 usFanGainHbm;
            public UInt16 usReserved;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_POWERTUNE_TABLE
        {
            public Byte ucRevId;
            public UInt16 usTDP;
            public UInt16 usConfigurableTDP;
            public UInt16 usTDC;
            public UInt16 usBatteryPowerLimit;
            public UInt16 usSmallPowerLimit;
            public UInt16 usLowCACLeakage;
            public UInt16 usHighCACLeakage;
            public UInt16 usMaximumPowerDeliveryLimit;
            public UInt16 usTjMax;
            public UInt16 usPowerTuneDataSetID;
            public UInt16 usEDCLimit;
            public UInt16 usSoftwareShutdownTemp;
            public UInt16 usClockStretchAmount;
            public UInt16 usTemperatureLimitHotspot;
            public UInt16 usTemperatureLimitLiquid1;
            public UInt16 usTemperatureLimitLiquid2;
            public UInt16 usTemperatureLimitVrVddc;
            public UInt16 usTemperatureLimitVrMvdd;
            public UInt16 usTemperatureLimitPlx;
            public Byte ucLiquid1_I2C_address;
            public Byte ucLiquid2_I2C_address;
            public Byte ucLiquid_I2C_Line;
            public Byte ucVr_I2C_address;
            public Byte ucVr_I2C_Line;
            public Byte ucPlx_I2C_address;
            public Byte ucPlx_I2C_Line;
            public UInt16 usReserved;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_VRAM_TIMING_ENTRY
        {
            public UInt32 ulClkRange;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
            public Byte[] ucLatency;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_VRAM_ENTRY
        {
            public UInt32 ulChannelMapCfg;
            public UInt16 usModuleSize;
            public UInt16 usMcRamCfg;
            public UInt16 usEnableChannels;
            public Byte ucExtMemoryID;
            public Byte ucMemoryType;
            public Byte ucChannelNum;
            public Byte ucChannelWidth;
            public Byte ucDensity;
            public Byte ucBankCol;
            public Byte ucMisc;
            public Byte ucVREFI;
            public UInt16 usReserved;
            public UInt16 usMemorySize;
            public Byte ucMcTunningSetId;
            public Byte ucRowNum;
            public UInt16 usEMRS2Value;
            public UInt16 usEMRS3Value;
            public Byte ucMemoryVenderID;
            public Byte ucRefreshRateFactor;
            public Byte ucFIFODepth;
            public Byte ucCDR_Bandwidth;
            public UInt32 ulChannelMapCfg1;
            public UInt32 ulBankMapCfg;
            public UInt32 ulReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public Byte[] strMemPNString;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ATOM_VRAM_INFO
        {
            public ATOM_COMMON_TABLE_HEADER sHeader;
            public UInt16 usMemAdjustTblOffset;
            public UInt16 usMemClkPatchTblOffset;
            public UInt16 usMcAdjustPerTileTblOffset;
            public UInt16 usMcPhyInitTableOffset;
            public UInt16 usDramDataRemapTblOffset;
            public UInt16 usReserved1;
            public Byte ucNumOfVRAMModule;
            public Byte ucMemoryClkPatchTblVer;
            public Byte ucVramModuleVer;
            public Byte ucMcPhyTileNum;
            // public ATOM_VRAM_ENTRY aVramInfo[ucNumOfVRAMModule];
        }
    }
}
