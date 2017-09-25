using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
namespace PolarisB
{
    public class BiosReader
    {
        public string Filepath { get; private set; }
        public int FSLength { get; private set; }
        public bool FSLength_Safe { get; private set; }




        //Some static info
        private string[] SupportedDeviceID = new string[] { "67DF", "1002" };
        private int[] SupportedRomLength = new int[] { 524288, 262144 };
        private int atom_rom_checksum_offset = 0x21;
        private int atom_rom_header_pointer = 0x48;
        private int atom_rom_timestamp_ponter = 0x50;


        private byte[] buffer; //Binaryreader Output

        //Store Read Vals in Arrays
        private int atom_rom_header_offset;
        public BiosStruct.ATOM_ROM_HEADER atom_rom_header { get; private set; }
        public BiosStruct.ATOM_DATA_TABLES atom_data_table { get; private set; }
        public BiosStruct.ATOM_POWERPLAY_TABLE atom_powerplay_table { get; private set; }
        public BiosStruct.ATOM_POWERTUNE_TABLE atom_powertune_table { get; private set; }
        public BiosStruct.ATOM_FAN_TABLE atom_fan_table { get; private set; }
        public BiosStruct.ATOM_MCLK_TABLE atom_mclk_table { get; private set; }
        public BiosStruct.ATOM_MCLK_ENTRY[] atom_mclk_entries { get; private set; }
        public BiosStruct.ATOM_SCLK_TABLE atom_sclk_table { get; private set; }
        public BiosStruct.ATOM_SCLK_ENTRY[] atom_sclk_entries { get; private set; }
        public BiosStruct.ATOM_VOLTAGE_TABLE atom_vddc_table { get; private set; }
        public BiosStruct.ATOM_VOLTAGE_ENTRY[] atom_vddc_entries { get; private set; }
        public BiosStruct.ATOM_VRAM_INFO atom_vram_info { get; private set; }
        public BiosStruct.ATOM_VRAM_TIMING_ENTRY[] atom_vram_timing_entries;
        public BiosStruct.ATOM_VRAM_ENTRY[] atom_vram_entries { get; private set; }
        int atom_vram_index = 0;

        public BiosReader(string filepath)
        {
            FileStream fs = new FileStream(filepath, FileMode.Open);
            //Get Bios Length

            this.Filepath = filepath;
            this.FSLength = (int)fs.Length;
            ValidateFileSize((int)fs.Length);
            using (BinaryReader br = new BinaryReader(fs))
            {
                //Read Bytes TO buffer
                buffer = br.ReadBytes(FSLength);

                ReadHeader();
                ValidateDeviceID();
                ReadValues();
            }
        }

        private void ValidateFileSize(int length)
        {
            foreach (var item in SupportedRomLength)
            {
                if (item != length && FSLength_Safe != true)
                {
                    FSLength_Safe = false;
                }
                else
                {
                    FSLength_Safe = true;
                }
            }
        }
        private void ReadHeader()
        {
            //Read Position Of ROM HEADER
            atom_rom_header_offset = Util.GetValueAtPosition(buffer, 16, BiosStruct.ATOM_ROM_HEADER_ptr);

            //READ HEADER
            atom_rom_header = Util.FromBytes<BiosStruct.ATOM_ROM_HEADER>(buffer.Skip(atom_rom_header_offset).ToArray());
        }
        private void ValidateDeviceID()
        {
            if (!SupportedDeviceID.Contains(atom_rom_header.usDeviceID.ToString("X")))
            {
                //something something not supported
            }
        }
        private void ReadValues()
        {
            atom_data_table             = Util.FromBytes<BiosStruct.ATOM_DATA_TABLES>(buffer.Skip(atom_rom_header.usMasterDataTableOffset).ToArray());
            atom_powerplay_table        = Util.FromBytes<BiosStruct.ATOM_POWERPLAY_TABLE>(buffer.Skip(atom_data_table.PowerPlayInfo).ToArray());
            atom_powertune_table        = Util.FromBytes<BiosStruct.ATOM_POWERTUNE_TABLE>(buffer.Skip(atom_data_table.PowerPlayInfo + atom_powerplay_table.usPowerTuneTableOffset).ToArray());
            atom_fan_table              = Util.FromBytes<BiosStruct.ATOM_FAN_TABLE>(buffer.Skip(atom_data_table.PowerPlayInfo + atom_powerplay_table.usFanTableOffset).ToArray());
            atom_mclk_table             = Util.FromBytes<BiosStruct.ATOM_MCLK_TABLE>(buffer.Skip(atom_data_table.PowerPlayInfo + atom_powerplay_table.usMclkDependencyTableOffset).ToArray());
            atom_mclk_entries           = new BiosStruct.ATOM_MCLK_ENTRY[atom_mclk_table.ucNumEntries];
            for (var i = 0; i < atom_mclk_entries.Length; i++){
            atom_mclk_entries[i]        = Util.FromBytes<BiosStruct.ATOM_MCLK_ENTRY>(buffer.Skip(atom_data_table.PowerPlayInfo + atom_powerplay_table.usMclkDependencyTableOffset + Marshal.SizeOf(typeof(BiosStruct.ATOM_MCLK_TABLE)) + Marshal.SizeOf(typeof(BiosStruct.ATOM_MCLK_ENTRY)) * i).ToArray());}
            atom_sclk_table             = Util.FromBytes<BiosStruct.ATOM_SCLK_TABLE>(buffer.Skip(atom_data_table.PowerPlayInfo + atom_powerplay_table.usSclkDependencyTableOffset).ToArray());
            atom_sclk_entries           = new BiosStruct.ATOM_SCLK_ENTRY[atom_sclk_table.ucNumEntries];
            for (var i = 0; i < atom_sclk_entries.Length; i++){
            atom_sclk_entries[i]        = Util.FromBytes<BiosStruct.ATOM_SCLK_ENTRY>(buffer.Skip(atom_data_table.PowerPlayInfo + atom_powerplay_table.usSclkDependencyTableOffset + Marshal.SizeOf(typeof(BiosStruct.ATOM_SCLK_TABLE)) + Marshal.SizeOf(typeof(BiosStruct.ATOM_SCLK_ENTRY)) * i).ToArray());}
            atom_vddc_table             = Util.FromBytes<BiosStruct.ATOM_VOLTAGE_TABLE>(buffer.Skip(atom_data_table.PowerPlayInfo + atom_powerplay_table.usVddcLookupTableOffset).ToArray());
            atom_vddc_entries           = new BiosStruct.ATOM_VOLTAGE_ENTRY[atom_vddc_table.ucNumEntries];
            for (var i = 0; i < atom_vddc_table.ucNumEntries; i++){
            atom_vddc_entries[i]        = Util.FromBytes<BiosStruct.ATOM_VOLTAGE_ENTRY>(buffer.Skip(atom_data_table.PowerPlayInfo + atom_powerplay_table.usVddcLookupTableOffset + Marshal.SizeOf(typeof(BiosStruct.ATOM_VOLTAGE_TABLE)) + Marshal.SizeOf(typeof(BiosStruct.ATOM_VOLTAGE_ENTRY)) * i).ToArray());}
            atom_vram_info              = Util.FromBytes<BiosStruct.ATOM_VRAM_INFO>(buffer.Skip(atom_data_table.VRAM_Info).ToArray());
            atom_vram_entries           = new BiosStruct.ATOM_VRAM_ENTRY[atom_vram_info.ucNumOfVRAMModule];
            //Old version supported mixed VRAM modules? Going to come back to this later
            for (var i = 0; i < atom_vram_info.ucNumOfVRAMModule; i++){
            atom_vram_entries[i]        = Util.FromBytes<BiosStruct.ATOM_VRAM_ENTRY>(buffer.Skip(atom_data_table.VRAM_Info + Marshal.SizeOf(typeof(BiosStruct.ATOM_VRAM_INFO))).ToArray());}
            atom_vram_timing_entries    = new BiosStruct.ATOM_VRAM_TIMING_ENTRY[16];
            for (var i = 0; i < 16; i++){
            atom_vram_timing_entries[i] = Util.FromBytes<BiosStruct.ATOM_VRAM_TIMING_ENTRY>(buffer.Skip(atom_data_table.VRAM_Info + 0x3D + Marshal.SizeOf(typeof(BiosStruct.ATOM_VRAM_TIMING_ENTRY)) * i).ToArray());
            if (atom_vram_timing_entries[i].ulClkRange == 0){ Array.Resize(ref atom_vram_timing_entries, i); break;}}
        }
    }
}
