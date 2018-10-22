using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace RFID_Reader
{
    public class MFRC522
    {
        #region fw_versions

        // Firmware data for self-test
        // Reference values based on firmware version
        // Hint: if needed, you can remove unused self-test data to save flash memory
        // Version 0.0 (0x90)
        // Philips Semiconductors; Preliminary Specification Revision 2.0 - 01 August 2005; 16.1 self-test
        private byte[] MFRC522_firmware_referenceV0_0 = new byte[]
        {
            0x00, 0x87, 0x98, 0x0f, 0x49, 0xFF, 0x07, 0x19,
            0xBF, 0x22, 0x30, 0x49, 0x59, 0x63, 0xAD, 0xCA,
            0x7F, 0xE3, 0x4E, 0x03, 0x5C, 0x4E, 0x49, 0x50,
            0x47, 0x9A, 0x37, 0x61, 0xE7, 0xE2, 0xC6, 0x2E,
            0x75, 0x5A, 0xED, 0x04, 0x3D, 0x02, 0x4B, 0x78,
            0x32, 0xFF, 0x58, 0x3B, 0x7C, 0xE9, 0x00, 0x94,
            0xB4, 0x4A, 0x59, 0x5B, 0xFD, 0xC9, 0x29, 0xDF,
            0x35, 0x96, 0x98, 0x9E, 0x4F, 0x30, 0x32, 0x8D
        };

        // Version 1.0 (0x91)
        // NXP Semiconductors; Rev. 3.8 - 17 September 2014; 16.1.1 self-test
        private byte[] MFRC522_firmware_referenceV1_0 = new byte[]
        {
            0x00, 0xC6, 0x37, 0xD5, 0x32, 0xB7, 0x57, 0x5C,
            0xC2, 0xD8, 0x7C, 0x4D, 0xD9, 0x70, 0xC7, 0x73,
            0x10, 0xE6, 0xD2, 0xAA, 0x5E, 0xA1, 0x3E, 0x5A,
            0x14, 0xAF, 0x30, 0x61, 0xC9, 0x70, 0xDB, 0x2E,
            0x64, 0x22, 0x72, 0xB5, 0xBD, 0x65, 0xF4, 0xEC,
            0x22, 0xBC, 0xD3, 0x72, 0x35, 0xCD, 0xAA, 0x41,
            0x1F, 0xA7, 0xF3, 0x53, 0x14, 0xDE, 0x7E, 0x02,
            0xD9, 0x0F, 0xB5, 0x5E, 0x25, 0x1D, 0x29, 0x79
        };

        // Version 2.0 (0x92)
        // NXP Semiconductors; Rev. 3.8 - 17 September 2014; 16.1.1 self-test
        private byte[] MFRC522_firmware_referenceV2_0 = new byte[]
        {
            0x00, 0xEB, 0x66, 0xBA, 0x57, 0xBF, 0x23, 0x95,
            0xD0, 0xE3, 0x0D, 0x3D, 0x27, 0x89, 0x5C, 0xDE,
            0x9D, 0x3B, 0xA7, 0x00, 0x21, 0x5B, 0x89, 0x82,
            0x51, 0x3A, 0xEB, 0x02, 0x0C, 0xA5, 0x00, 0x49,
            0x7C, 0x84, 0x4D, 0xB3, 0xCC, 0xD2, 0x1B, 0x81,
            0x5D, 0x48, 0x76, 0xD5, 0x71, 0x61, 0x21, 0xA9,
            0x86, 0x96, 0x83, 0x38, 0xCF, 0x9D, 0x5B, 0x6D,
            0xDC, 0x15, 0xBA, 0x3E, 0x7D, 0x95, 0x3B, 0x2F
        };

        // Clone
        // Fudan Semiconductor FM17522 (0x88)
        private byte[] FM17522_firmware_reference = new byte[]
        {
            0x00, 0xD6, 0x78, 0x8C, 0xE2, 0xAA, 0x0C, 0x18,
            0x2A, 0xB8, 0x7A, 0x7F, 0xD3, 0x6A, 0xCF, 0x0B,
            0xB1, 0x37, 0x63, 0x4B, 0x69, 0xAE, 0x91, 0xC7,
            0xC3, 0x97, 0xAE, 0x77, 0xF4, 0x37, 0xD7, 0x9B,
            0x7C, 0xF5, 0x3C, 0x11, 0x8F, 0x15, 0xC3, 0xD7,
            0xC1, 0x5B, 0x00, 0x2A, 0xD0, 0x75, 0xDE, 0x9E,
            0x51, 0x64, 0xAB, 0x3E, 0xE9, 0x15, 0xB5, 0xAB,
            0x56, 0x9A, 0x98, 0x82, 0x26, 0xEA, 0x2A, 0x62
        };

        #endregion

        #region definitions

        public byte[,] knownKeys = new byte[16, 6] {
            {0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5}, // A0 A1 A2 A3 A4 A5
            {0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5}, // B0 B1 B2 B3 B4 B5
            {0x4d, 0x3a, 0x99, 0xc3, 0x51, 0xdd}, // 4D 3A 99 C3 51 DD
            {0x1a, 0x98, 0x2c, 0x7e, 0x45, 0x9a}, // 1A 98 2C 7E 45 9A
            {0xd3, 0xf7, 0xd3, 0xf7, 0xd3, 0xf7}, // D3 F7 D3 F7 D3 F7
            {0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff}, // AA BB CC DD EE FF
            {0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, // 00 00 00 00 00 00
            {0xd3, 0xf7, 0xd3, 0xf7, 0xd3, 0xf7}, // d3 f7 d3 f7 d3 f7
            {0xa0, 0xb0, 0xc0, 0xd0, 0xe0, 0xf0}, // a0 b0 c0 d0 e0 f0
            {0xa1, 0xb1, 0xc1, 0xd1, 0xe1, 0xf1}, // a1 b1 c1 d1 e1 f1
            {0x71, 0x4c, 0x5c, 0x88, 0x6e, 0x97}, // 71 4c 5c 88 6e 97
            {0x58, 0x7e, 0xe5, 0xf9, 0x35, 0x0f}, // 58 7e e5 f9 35 0f
            {0xa0, 0x47, 0x8c, 0xc3, 0x90, 0x91}, // a0 47 8c c3 90 91
            {0x53, 0x3c, 0xb6, 0xc7, 0x23, 0xf6}, // 53 3c b6 c7 23 f6
            {0x8f, 0xd0, 0xa4, 0xf2, 0x56, 0xe9}, // 8f d0 a4 f2 56 e9
            {0xff, 0xff, 0xff, 0xff, 0xff, 0xff}  // FF FF FF FF FF FF
        };

        // Size of the MFRC522 FIFO
        private const byte FIFO_SIZE = 64;       // The FIFO is 64 bytes.

        // MFRC522 registers. Described in chapter 9 of the datasheet.
        public enum PCD_Register : byte
        {
            // Page 0: Command and status
            //						  0x00			// reserved for future use
            CommandReg = 0x01, // starts and stops command execution
            ComIEnReg = 0x02,  // enable and disable interrupt request control bits
            DivIEnReg = 0x03,  // enable and disable interrupt request control bits
            ComIrqReg = 0x04,  // interrupt request bits
            DivIrqReg = 0x05,  // interrupt request bits
            ErrorReg = 0x06,   // error bits showing the error status of the last command executed 
            Status1Reg = 0x07, // communication status bits
            Status2Reg = 0x08, // receiver and transmitter status bits
            FIFODataReg = 0x09,    // input and output of 64 byte FIFO buffer
            FIFOLevelReg = 0x0A,   // number of bytes stored in the FIFO buffer
            WaterLevelReg = 0x0B,  // level for FIFO underflow and overflow warning
            ControlReg = 0x0C, // miscellaneous control registers
            BitFramingReg = 0x0D,  // adjustments for bit-oriented frames
            CollReg = 0x0E,    // bit position of the first bit-collision detected on the RF interface
                               //						  0x0F			// reserved for future use

            // Page 1: Command
            // 						  0x10			// reserved for future use
            ModeReg = 0x11,    // defines general modes for transmitting and receiving 
            TxModeReg = 0x12,  // defines transmission data rate and framing
            RxModeReg = 0x13,  // defines reception data rate and framing
            TxControlReg = 0x14,   // controls the logical behavior of the antenna driver pins TX1 and TX2
            TxASKReg = 0x15,   // controls the setting of the transmission modulation
            TxSelReg = 0x16,   // selects the internal sources for the antenna driver
            RxSelReg = 0x17,   // selects internal receiver settings
            RxThresholdReg = 0x18, // selects thresholds for the bit decoder
            DemodReg = 0x19,   // defines demodulator settings
                               // 						  0x1A			// reserved for future use
                               // 						  0x1B			// reserved for future use
            MfTxReg = 0x1C,    // controls some MIFARE communication transmit parameters
            MfRxReg = 0x1D,    // controls some MIFARE communication receive parameters
                               // 						  0x1E			// reserved for future use
            SerialSpeedReg = 0x1F, // selects the speed of the serial UART interface

            // Page 2: Configuration
            // 						  0x20			// reserved for future use
            CRCResultRegH = 0x21,  // shows the MSB and LSB values of the CRC calculation
            CRCResultRegL = 0x22,
            // 						  0x23			// reserved for future use
            ModWidthReg = 0x24,    // controls the ModWidth setting?
                                   // 						  0x25			// reserved for future use
            RFCfgReg = 0x26,   // configures the receiver gain
            GsNReg = 0x27, // selects the conductance of the antenna driver pins TX1 and TX2 for modulation 
            CWGsPReg = 0x28,   // defines the conductance of the p-driver output during periods of no modulation
            ModGsPReg = 0x29,  // defines the conductance of the p-driver output during periods of modulation
            TModeReg = 0x2A,   // defines settings for the internal timer
            TPrescalerReg = 0x2B,  // the lower 8 bits of the TPrescaler value. The 4 high bits are in TModeReg.
            TReloadRegH = 0x2C,    // defines the 16-bit timer reload value
            TReloadRegL = 0x2D,
            TCounterValueRegH = 0x2E,  // shows the 16-bit timer value
            TCounterValueRegL = 0x2F,

            // Page 3: Test Registers
            // 						  0x30			// reserved for future use
            TestSel1Reg = 0x31,    // general test signal configuration
            TestSel2Reg = 0x32,    // general test signal configuration
            TestPinEnReg = 0x33,   // enables pin output driver on pins D1 to D7
            TestPinValueReg = 0x34,    // defines the values for D1 to D7 when it is used as an I/O bus
            TestBusReg = 0x35, // shows the status of the internal test bus
            AutoTestReg = 0x36,    // controls the digital self-test
            VersionReg = 0x37, // shows the software version
            AnalogTestReg = 0x38,  // controls the pins AUX1 and AUX2
            TestDAC1Reg = 0x39,    // defines the test value for TestDAC1
            TestDAC2Reg = 0x3A,    // defines the test value for TestDAC2
            TestADCReg = 0x3B       // shows the value of ADC I and Q channels
                                    // 						  0x3C			// reserved for production tests
                                    // 						  0x3D			// reserved for production tests
                                    // 						  0x3E			// reserved for production tests
                                    // 						  0x3F			// reserved for production tests
        };

        // MFRC522 commands. Described in chapter 10 of the datasheet.
        public enum PCD_Command : byte
        {
            PCD_Idle = 0x00,        // no action, cancels current command execution
            PCD_Mem = 0x01,     // stores 25 bytes into the internal buffer
            PCD_GenerateRandomID = 0x02,        // generates a 10-byte random ID number
            PCD_CalcCRC = 0x03,     // activates the CRC coprocessor or performs a self-test
            PCD_Transmit = 0x04,        // transmits data from the FIFO buffer
            PCD_NoCmdChange = 0x07,     // no command change, can be used to modify the CommandReg register bits without affecting the command, for example, the PowerDown bit
            PCD_Receive = 0x08,     // activates the receiver circuits
            PCD_Transceive = 0x0C,      // transmits data from FIFO buffer to antenna and automatically activates the receiver after transmission
            PCD_MFAuthent = 0x0E,       // performs the MIFARE standard authentication as a reader
            PCD_SoftReset = 0x0F        // resets the MFRC522
        };

        // MFRC522 RxGain[2:0] masks, defines the receiver's signal voltage gain factor (on the PCD).
        // Described in 9.3.3.6 / table 98 of the datasheet at http://www.nxp.com/documents/data_sheet/MFRC522.pdf
        public enum PCD_RxGain : byte
        {
            RxGain_18dB = 0x00 << 4,    // 000b - 18 dB, minimum
            RxGain_23dB = 0x01 << 4,    // 001b - 23 dB
            RxGain_18dB_2 = 0x02 << 4,  // 010b - 18 dB, it seems 010b is a duplicate for 000b
            RxGain_23dB_2 = 0x03 << 4,  // 011b - 23 dB, it seems 011b is a duplicate for 001b
            RxGain_33dB = 0x04 << 4,    // 100b - 33 dB, average, and typical default
            RxGain_38dB = 0x05 << 4,    // 101b - 38 dB
            RxGain_43dB = 0x06 << 4,    // 110b - 43 dB
            RxGain_48dB = 0x07 << 4,    // 111b - 48 dB, maximum
            RxGain_min = 0x00 << 4, // 000b - 18 dB, minimum, convenience for RxGain_18dB
            RxGain_avg = 0x04 << 4, // 100b - 33 dB, average, convenience for RxGain_33dB
            RxGain_max = 0x07 << 4      // 111b - 48 dB, maximum, convenience for RxGain_48dB
        };

        // Commands sent to the PICC.
        public enum PICC_Command : byte
        {
            // The commands used by the PCD to manage communication with several PICCs (ISO 14443-3, Type A, section 6.4)
            PICC_CMD_REQA = 0x26,       // REQuest command, Type A. Invites PICCs in state IDLE to go to READY and prepare for anticollision or selection. 7 bit frame.
            PICC_CMD_WUPA = 0x52,       // Wake-UP command, Type A. Invites PICCs in state IDLE and HALT to go to READY(*) and prepare for anticollision or selection. 7 bit frame.
            PICC_CMD_CT = 0x88,     // Cascade Tag. Not really a command, but used during anti collision.
            PICC_CMD_SEL_CL1 = 0x93,        // Anti collision/Select, Cascade Level 1
            PICC_CMD_SEL_CL2 = 0x95,        // Anti collision/Select, Cascade Level 2
            PICC_CMD_SEL_CL3 = 0x97,        // Anti collision/Select, Cascade Level 3
            PICC_CMD_HLTA = 0x50,       // HaLT command, Type A. Instructs an ACTIVE PICC to go to state HALT.
            PICC_CMD_RATS = 0xE0,     // Request command for Answer To Reset.
                                      // The commands used for MIFARE Classic (from http://www.mouser.com/ds/2/302/MF1S503x-89574.pdf, Section 9)
                                      // Use PCD_MFAuthent to authenticate access to a sector, then use these commands to read/write/modify the blocks on the sector.
                                      // The read/write commands can also be used for MIFARE Ultralight.
            PICC_CMD_MF_AUTH_KEY_A = 0x60,      // Perform authentication with Key A
            PICC_CMD_MF_AUTH_KEY_B = 0x61,      // Perform authentication with Key B
            PICC_CMD_MF_READ = 0x30,        // Reads one 16 byte block from the authenticated sector of the PICC. Also used for MIFARE Ultralight.
            PICC_CMD_MF_WRITE = 0xA0,       // Writes one 16 byte block to the authenticated sector of the PICC. Called "COMPATIBILITY WRITE" for MIFARE Ultralight.
            PICC_CMD_MF_DECREMENT = 0xC0,       // Decrements the contents of a block and stores the result in the internal data register.
            PICC_CMD_MF_INCREMENT = 0xC1,       // Increments the contents of a block and stores the result in the internal data register.
            PICC_CMD_MF_RESTORE = 0xC2,     // Reads the contents of a block into the internal data register.
            PICC_CMD_MF_TRANSFER = 0xB0,        // Writes the contents of the internal data register to a block.
                                                // The commands used for MIFARE Ultralight (from http://www.nxp.com/documents/data_sheet/MF0ICU1.pdf, Section 8.6)
                                                // The PICC_CMD_MF_READ and PICC_CMD_MF_WRITE can also be used for MIFARE Ultralight.
            PICC_CMD_UL_WRITE = 0xA2        // Writes one 4 byte page to the PICC.
        };

        // MIFARE constants that does not fit anywhere else
        public enum MIFARE_Misc
        {
            MF_ACK = 0x0A,       // The MIFARE Classic uses a 4 bit ACK/NAK. Any other value than 0x0A is NAK.
            MF_KEY_SIZE = 6         // A Mifare Crypto1 key is 6 bytes.
        };

        public enum PICC_Type : byte
        {
            PICC_TYPE_UNKNOWN,
            PICC_TYPE_ISO_14443_4,  // PICC compliant with ISO/IEC 14443-4 
            PICC_TYPE_ISO_18092,    // PICC compliant with ISO/IEC 18092 (NFC)
            PICC_TYPE_MIFARE_MINI,  // MIFARE Classic protocol, 320 bytes
            PICC_TYPE_MIFARE_1K,    // MIFARE Classic protocol, 1KB
            PICC_TYPE_MIFARE_4K,    // MIFARE Classic protocol, 4KB
            PICC_TYPE_MIFARE_UL,    // MIFARE Ultralight or Ultralight C
            PICC_TYPE_MIFARE_PLUS,  // MIFARE Plus
            PICC_TYPE_MIFARE_DESFIRE,   // MIFARE DESFire
            PICC_TYPE_TNP3XXX,  // Only mentioned in NXP AN 10833 MIFARE Type Identification Procedure
            PICC_TYPE_NOT_COMPLETE = 0xff   // SAK indicates UID is not complete.
        };

        public enum StatusCode : byte
        {
            STATUS_OK,  // Success
            STATUS_ERROR,   // Error in communication
            STATUS_COLLISION,   // Collission detected
            STATUS_TIMEOUT, // Timeout in communication.
            STATUS_NO_ROOM, // A buffer is not big enough.
            STATUS_INTERNAL_ERROR,  // Internal error in the code. Should not happen ;-)
            STATUS_INVALID, // Invalid argument.
            STATUS_CRC_WRONG,   // The CRC_A does not match
            STATUS_MIFARE_NACK = 0xff   // A MIFARE PICC responded with NAK.
        };

        public struct Uid
        {
            public byte size;          // Number of bytes in the UID. 4, 7 or 10.
            public byte[] uidByte;
            public byte sak;           // The SAK (Select acknowledge) byte returned from the PICC after successful selection.
        };

        // A struct used for passing a MIFARE Crypto1 key
        public byte[] MIFARE_Key = new byte[(int)MIFARE_Misc.MF_KEY_SIZE];
        public Uid uid;                                // Used by PICC_ReadCardSerial().
        #endregion

        private SerialPort serialPort1 = new SerialPort();

        public bool IsConnected = false;

        public void Open(string comName, int comSpeed)
        {
            IsConnected = false;
            if (serialPort1.IsOpen) serialPort1.Close();
            serialPort1.PortName = comName;
            serialPort1.BaudRate = 9600;
            try
            {
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                serialPort1.Close();
                MessageBox.Show("Error opening port " + serialPort1.PortName + ": " + ex.Message);
            }
            IsConnected = true;
        }

        public void Close()
        {
            IsConnected = false;
            try
            {
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error closing port " + serialPort1.PortName + ": " + ex.Message);
            }
        }

        public MFRC522()
        {
            serialPort1.DataBits = 8;
            serialPort1.Handshake = Handshake.None;
            serialPort1.Parity = Parity.None;
            serialPort1.StopBits = StopBits.One;
            serialPort1.ReadTimeout = 100;
            serialPort1.WriteTimeout = 100;
        }

        #region Basic interface functions for communicating with the MFRC522

        // Writes a byte to the specified register in the MFRC522 chip.
        private bool PCD_WriteRegister(byte reg, byte value)
        {
            byte tmp = 0;
            int count = 0;
            while (true)
            {
                try
                {
                    serialPort1.DiscardInBuffer();
                    serWrite((byte)(reg & 127));
                    serWrite(value);
                    tmp = (byte)serialPort1.ReadByte();
                }
                catch
                {
                    return false;
                }
                if (tmp == reg)
                {
                    return true;
                }
                count++;
                if (count > 10)
                {
                    //MessageBox.Show("Error writing register: 0x" + addr.ToString("X2"));
                    return false;
                }
            }
        }

        // Writes a number of bytes to the specified register in the MFRC522 chip.
        private bool PCD_WriteRegister(byte reg, int count, byte[] value)
        {
            serialPort1.DiscardInBuffer();
            for (int txBytes = 0; txBytes < count; txBytes++)
            {
                if (!PCD_WriteRegister(reg, value[txBytes])) return false;
            }
            return true;
        }

        // Reads a byte from the specified register in the MFRC522 chip.
        private byte PCD_ReadRegister(byte reg)
        {
            byte val = 0;
            try
            {
                serialPort1.DiscardInBuffer();
                serWrite((byte)(reg + 0x80));
                val = (byte)serialPort1.ReadByte();
            }
            catch
            {
                return 0;
            }
            return val;
        }

        // Reads a number of bytes from the specified register in the MFRC522 chip.
        // Only bit positions rxAlign..7 in values[0] are updated.
        private byte[] PCD_ReadRegister(byte reg, byte count, byte rxAlign)
        {
            byte[] values = new byte[count];
            if (count == 0)
            {
                return null;
            }
            byte index = 0;

            if (rxAlign > 0)
            {       // Only update bit positions rxAlign..7 in values[0]
                    // Create bit mask for bit positions rxAlign..7
                byte mask = (byte)((0xFF << rxAlign) & 0xFF);
                // Read value and tell that we want to read the same address again.
                byte value = PCD_ReadRegister(reg);
                // Apply mask to both current value of values[0] and the new data in value.
                values[0] = (byte)((byte)(values[0] & ~mask) | (byte)(value & mask));
                index++;
            }
            while (index < count)
            {
                values[index] = PCD_ReadRegister(reg);
                index++;
            }
            return values;
        }

        // Sets the bits given in mask in register reg.
        private bool PCD_SetRegisterBitMask(byte reg, byte mask)
        {
            byte tmp = PCD_ReadRegister(reg);
            return PCD_WriteRegister(reg, (byte)(tmp | mask));
        }

        // Clears the bits given in mask from register reg.
        private bool PCD_ClearRegisterBitMask(byte reg, byte mask)
        {
            byte tmp = PCD_ReadRegister(reg);
            return PCD_WriteRegister(reg, (byte)(tmp & (~mask)));
        }

        // Use the CRC coprocessor in the MFRC522 to calculate a CRC_A.
        private StatusCode PCD_CalculateCRC(byte[] data, byte length, out byte[] result)
        {
            PCD_WriteRegister((byte)PCD_Register.CommandReg, (byte)PCD_Command.PCD_Idle); // Stop any active command.
            PCD_WriteRegister((byte)PCD_Register.DivIrqReg, 0x04); // Clear the CRCIRq interrupt request bit
            PCD_WriteRegister((byte)PCD_Register.FIFOLevelReg, 0x80);
            PCD_WriteRegister((byte)PCD_Register.FIFODataReg, length, data);
            PCD_WriteRegister((byte)PCD_Register.CommandReg, (byte)PCD_Command.PCD_CalcCRC);

            result = new byte[2];

            const int timeout = 100;// create timer for timeout (just in case)
            DateTime start = DateTime.Now;
            // set timeout to 100 ms
            while (DateTime.Now.Subtract(start).TotalMilliseconds < timeout)
            {
                byte n = PCD_ReadRegister((byte)PCD_Register.DivIrqReg);
                if ((n & 4) != 0)
                {
                    PCD_WriteRegister((byte)PCD_Register.CommandReg, (byte)PCD_Command.PCD_Idle);
                    result[0] = PCD_ReadRegister((byte)PCD_Register.CRCResultRegL);
                    result[1] = PCD_ReadRegister((byte)PCD_Register.CRCResultRegH);
                    return StatusCode.STATUS_OK;
                }
            }
            // 89ms passed and nothing happend. Communication with the MFRC522 might be down.
            return StatusCode.STATUS_TIMEOUT;
        }

        #endregion

        #region manipulating the MFRC522

        //Initializes the MFRC522 chip.
        public bool PCD_Init(bool DTR_RESET_CONNECTED = false)
        {
            bool hardReset = false;
            //perform a gardwire reset if DTR connected
            if (DTR_RESET_CONNECTED)
            {
                serialPort1.DtrEnable = false;
                Delay_ms(100);
                serialPort1.DtrEnable = true;
                Delay_ms(50);
                hardReset = true;
            }

            if (!hardReset) // Perform a soft reset if we haven't triggered a hard reset above.
            {
                PCD_Reset();
            }
            bool status = true;
            status &= PCD_WriteRegister((byte)PCD_Register.TxModeReg, 0x00);
            status &= PCD_WriteRegister((byte)PCD_Register.RxModeReg, 0x00);
            // Reset ModWidthReg
            status &= PCD_WriteRegister((byte)PCD_Register.ModWidthReg, 0x26);
            // When communicating with a PICC we need a timeout if something goes wrong.
            // f_timer = 13.56 MHz / (2*TPreScaler+1) where TPreScaler = [TPrescaler_Hi:TPrescaler_Lo].
            // TPrescaler_Hi are the four low bits in TModeReg. TPrescaler_Lo is TPrescalerReg.
            status &= PCD_WriteRegister((byte)PCD_Register.TModeReg, 0x80);          // TAuto=1; timer starts automatically at the end of the transmission in all communication modes at all speeds
            status &= PCD_WriteRegister((byte)PCD_Register.TPrescalerReg, 0xA9);     // TPreScaler = TModeReg[3..0]:TPrescalerReg, ie 0x0A9 = 169 => f_timer=40kHz, ie a timer period of 25Ојs.
            status &= PCD_WriteRegister((byte)PCD_Register.TReloadRegH, 0x03);       // Reload timer with 0x3E8 = 1000, ie 25ms before timeout.
            status &= PCD_WriteRegister((byte)PCD_Register.TReloadRegL, 0xE8);
            status &= PCD_WriteRegister((byte)PCD_Register.TxASKReg, 0x40);      // Default 0x00. Force a 100 % ASK modulation independent of the ModGsPReg register setting
            status &= PCD_WriteRegister((byte)PCD_Register.ModeReg, 0x3D);       // Default 0x3F. Set the preset value for the CRC coprocessor for the CalcCRC command to 0x6363 (ISO 14443-3 part 6.2.4)
            status &= PCD_AntennaOn();						// Enable the antenna driver pins TX1 and TX2 (they were disabled by the reset)
            return status;
        }

        public bool PCD_Reset()
        {
            bool status = true;
            status &= PCD_WriteRegister((byte)PCD_Register.CommandReg, (byte)PCD_Command.PCD_SoftReset);// Issue the SoftReset command.
                                                                                                        // The datasheet does not mention how long the SoftRest command takes to complete.
                                                                                                        // But the MFRC522 might have been in soft power-down mode (triggered by bit 4 of CommandReg) 
                                                                                                        // Section 8.8.2 in the datasheet says the oscillator start-up time is the start up time of the crystal + 37,74Ојs. Let us be generous: 50ms.
            byte count = 0;
            do
            {
                // Wait for the PowerDown bit in CommandReg to be cleared (max 3x50ms)
                Delay_ms(50);

            } while ((PCD_ReadRegister((byte)PCD_Register.CommandReg) & (1 << 4)) != 0 && (++count < 3));
            if (count >= 3) return false;
            else return true;
        }

        public void PCD_HwReset()
        {
            serialPort1.DtrEnable = false;
            Delay_ms(100);
            serialPort1.DtrEnable = true;
            Delay_ms(50);
        }

        public bool PCD_AntennaOn()
        {
            bool status = true;
            byte value = PCD_ReadRegister((byte)PCD_Register.TxControlReg);
            if ((value & 0x03) != 0x03)
            {
                status &= PCD_WriteRegister((byte)PCD_Register.TxControlReg, (byte)(value | 0x03));
            }
            return status;
        }

        public bool PCD_AntennaOff()
        {
            return PCD_ClearRegisterBitMask((byte)PCD_Register.TxControlReg, 0x03);
        }

        public byte PCD_GetAntennaGain()
        {
            return (byte)(PCD_ReadRegister((byte)PCD_Register.RFCfgReg) & (0x07 << 4));
        }

        public bool PCD_SetAntennaGain(byte mask)
        {
            bool status = true;
            if (PCD_GetAntennaGain() != mask)
            {
                status &= PCD_ClearRegisterBitMask((byte)PCD_Register.RFCfgReg, (0x07 << 4));
                status &= PCD_SetRegisterBitMask((byte)PCD_Register.RFCfgReg, (byte)(mask & (0x07 << 4)));
            }
            return status;
        }

        public bool PCD_PerformSelfTest()
        {
            bool status = true;
            // This follows directly the steps outlined in 16.1.1
            // 1. Perform a soft reset.
            status &= PCD_Reset();

            // 2. Clear the internal buffer by writing 25 bytes of 00h
            byte[] ZEROES = new byte[25];
            status &= PCD_WriteRegister((byte)PCD_Register.FIFOLevelReg, 0x80);      // flush the FIFO buffer
            status &= PCD_WriteRegister((byte)PCD_Register.FIFODataReg, 25, ZEROES); // write 25 bytes of 00h to FIFO
            status &= PCD_WriteRegister((byte)PCD_Register.CommandReg, (byte)PCD_Command.PCD_Mem);     // transfer to internal buffer

            // 3. Enable self-test
            status &= PCD_WriteRegister((byte)PCD_Register.AutoTestReg, 0x09);

            // 4. Write 00h to FIFO buffer
            status &= PCD_WriteRegister((byte)PCD_Register.FIFODataReg, 0x00);

            // 5. Start self-test by issuing the CalcCRC command
            status &= PCD_WriteRegister((byte)PCD_Register.CommandReg, (byte)PCD_Command.PCD_CalcCRC);

            // 6. Wait for self-test to complete
            byte n;
            for (byte i = 0; i < 0xFF; i++)
            {
                // The datasheet does not specify exact completion condition except
                // that FIFO buffer should contain 64 bytes.
                // While selftest is initiated by CalcCRC command
                // it behaves differently from normal CRC computation,
                // so one can't reliably use DivIrqReg to check for completion.
                // It is reported that some devices does not trigger CRCIRq flag
                // during selftest.
                n = PCD_ReadRegister((byte)PCD_Register.FIFOLevelReg);
                if (n >= 64)
                {
                    break;
                }
            }
            status &= PCD_WriteRegister((byte)PCD_Register.CommandReg, (byte)PCD_Command.PCD_Idle);        // Stop calculating CRC for new content in the FIFO.

            // 7. Read out resulting 64 bytes from the FIFO buffer.
            byte[] result = new byte[64];
            result = PCD_ReadRegister((byte)PCD_Register.FIFODataReg, 64, 0);

            // Auto self-test done
            // Reset AutoTestReg register to be 0 again. Required for normal operation.
            status &= PCD_WriteRegister((byte)PCD_Register.AutoTestReg, 0x00);

            // Determine firmware version (see section 9.3.4.8 in spec)
            byte version = PCD_ReadRegister((byte)PCD_Register.VersionReg);

            // Pick the appropriate reference values
            byte[] reference = new byte[64];
            switch (version)
            {
                case 0x88:  // Fudan Semiconductor FM17522 clone
                    reference = FM17522_firmware_reference;
                    break;
                case 0x90:  // Version 0.0
                    reference = MFRC522_firmware_referenceV0_0;
                    break;
                case 0x91:  // Version 1.0
                    reference = MFRC522_firmware_referenceV1_0;
                    break;
                case 0x92:  // Version 2.0
                    reference = MFRC522_firmware_referenceV2_0;
                    break;
                default:    // Unknown version
                    return false; // abort test
            }
            return ByteArrayCompare(reference, result);
        }

        #endregion

        #region Power control

        //IMPORTANT NOTE!!!!
        //Calling any other function that uses CommandReg will disable soft power down mode !!!
        //For more details about power control, refer to the datasheet - page 33 (8.6)

        //Note : Only soft power down mode is available throught software
        public void PCD_SoftPowerDown()
        {
            byte val = PCD_ReadRegister((byte)PCD_Register.CommandReg); // Read state of the command register 
            val |= (1 << 4);// set PowerDown bit ( bit 4 ) to 1 
            PCD_WriteRegister((byte)PCD_Register.CommandReg, val);//write new value to the command register
        }

        public void PCD_SoftPowerUp()
        {
            byte val = PCD_ReadRegister((byte)PCD_Register.CommandReg); // Read state of the command register 

            unchecked
            {
                val &= (byte)~(1 << 4);// set PowerDown bit ( bit 4 ) to 0 
            }
            //val = Accessory.ClearBit(val, 4);

            PCD_WriteRegister((byte)PCD_Register.CommandReg, val);//write new value to the command register
                                                                  // wait until PowerDown bit is cleared (this indicates end of wake up procedure) 

            const int timeout = 500;// create timer for timeout (just in case) 
            DateTime start = DateTime.Now;
            // set timeout to 500 ms 
            while (DateTime.Now.Subtract(start).TotalMilliseconds < timeout)
            {
                val = PCD_ReadRegister((byte)PCD_Register.CommandReg);// Read state of the command register
                if ((val & (1 << 4)) > 0)
                //if (!Accessory.GetBit(val, 4)) // if powerdown bit is 0 
                {
                    break;// wake up procedure is finished 
                }
            }
        }

        #endregion

        #region communicating with PICCs

        //**** global output: backData, backLen, validBits
        private StatusCode PCD_TransceiveData(byte[] sendData,    	///< Pointer to the data to transfer to the FIFO.
													byte sendLen,		    ///< Number of bytes to transfer to the FIFO.
													out byte[] backData,	///< NULL or pointer to buffer if data should be read back after executing the command.
													ref byte backLen,		    ///< In: Max number of bytes to write to *backData. Out: The number of bytes returned.                                                    
                                                    ref byte validBits,     	///< In: The number of valid bits in the last byte. 0 for 8 valid bits. Default NULL.                                                    
													byte rxAlign = 0,	    	///< In: Defines the bit position in backData[0] for the first bit received. Default 0.
													bool checkCRC = false	    	///< In: True => The last two bytes of the response is assumed to be a CRC_A that must be validated.
								 )
        {
            byte waitIRq = 0x30;        // RxIRq and IdleIRq
            backData = new byte[backLen];
            StatusCode ret = PCD_CommunicateWithPICC((byte)PCD_Command.PCD_Transceive, waitIRq, sendData, sendLen, out backData, ref backLen, ref validBits, rxAlign, checkCRC);

            return ret;
        }

        //**** global output: backData, backLen, validBits
        private StatusCode PCD_CommunicateWithPICC(byte command,		///< The command to execute. One of the PCD_Command enums.
														byte waitIRq,		///< The bits in the ComIrqReg register that signals successful completion of the command.
														byte[] sendData,	///< Pointer to the data to transfer to the FIFO.
														byte sendLen,		///< Number of bytes to transfer to the FIFO.
														out byte[] backData,	///< NULL or pointer to buffer if data should be read back after executing the command.
														ref byte backLen,     ///< In: Max number of bytes to write to *backData. Out: The number of bytes returned.
                                                        ref byte validBits,	    ///< In/Out: The number of valid bits in the last byte. 0 for 8 valid bits.                                                        
                                                        byte rxAlign = 0,		///< In: Defines the bit position in backData[0] for the first bit received. Default 0.
														bool checkCRC = false		///< In: True => The last two bytes of the response is assumed to be a CRC_A that must be validated.
									 )
        {
            backData = new byte[backLen];
            // Prepare values for BitFramingReg
            byte txLastBits = 0;
            if (validBits != 0) txLastBits = validBits;
            byte bitFraming = (byte)((rxAlign << 4) + txLastBits);      // RxAlign = BitFramingReg[6..4]. TxLastBits = BitFramingReg[2..0]

            /*byte irqEn = 0;
            if (command == (byte)PCD_Command.PCD_Transceive)
            {
                irqEn = 0x77;
                waitIRq = 0x30;
            }
            else if (command == (byte)PCD_Command.PCD_MFAuthent)
            {
                irqEn = 0x12;
                waitIRq = 0x10;
            }*/


            bool s = true;
            s &= PCD_WriteRegister((byte)PCD_Register.CommandReg, (byte)PCD_Command.PCD_Idle);            // Stop any active command. ???
            s &= PCD_WriteRegister((byte)PCD_Register.ComIrqReg, 0x7F);                 // Clear all seven interrupt request bits ???
            s &= PCD_WriteRegister((byte)PCD_Register.FIFOLevelReg, 0x80);              // FlushBuffer = 1, FIFO initialization
            s &= PCD_WriteRegister((byte)PCD_Register.FIFODataReg, sendLen, sendData);  // Write sendData to the FIFO
            s &= PCD_WriteRegister((byte)PCD_Register.BitFramingReg, bitFraming);       // Bit adjustments
            s &= PCD_WriteRegister((byte)PCD_Register.CommandReg, command);             // Execute the command
            if (command == (byte)PCD_Command.PCD_Transceive)
            {
                PCD_SetRegisterBitMask((byte)PCD_Register.BitFramingReg, 0x80);    // StartSend=1, transmission of data starts
            }

            // Wait for the command to complete.
            // In PCD_Init() we set the TAuto flag in TModeReg. This means the timer automatically starts when the PCD stops transmitting.
            // Each iteration of the do-while-loop takes 17.86μs.
            // TODO check/modify for other architectures than Arduino Uno 16bit
            // set timeout to 50 ms

            int i = 0;
            const int timeout = 100;// create timer for timeout (just in case)
            DateTime start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < timeout)
            //i = 2000;
            //while (i >= 1)
            {
                byte n = PCD_ReadRegister((byte)PCD_Register.ComIrqReg);   // ComIrqReg[7..0] bits are: Set1 TxIRq RxIRq IdleIRq HiAlertIRq LoAlertIRq ErrIRq TimerIRq
                if ((n & waitIRq) != 0)
                {                   // One of the interrupts that signal success has been set.
                    i = 1;
                    break;
                }
                if ((n & 0x01) != 0)
                {                       // Timer interrupt - nothing received in 25ms
                    return StatusCode.STATUS_TIMEOUT;
                }
                //i--;

            } // 35.7ms and nothing happend. Communication with the MFRC522 might be down.
            if (i == 0)
            {
                return StatusCode.STATUS_TIMEOUT;
            }

            // Stop now if any errors except collisions were detected.
            byte errorRegValue = PCD_ReadRegister((byte)PCD_Register.ErrorReg); // ErrorReg[7..0] bits are: WrErr TempErr reserved BufferOvfl CollErr CRCErr ParityErr ProtocolErr
            if ((errorRegValue & 0x13) > 0)
            {    // BufferOvfl ParityErr ProtocolErr
                return StatusCode.STATUS_ERROR;
            }

            byte _validBits = 0;

            // If the caller wants data back, get it from the MFRC522.
            if (backData.Length > 0 && backLen > 0)
            {
                byte n = PCD_ReadRegister((byte)PCD_Register.FIFOLevelReg);    // Number of bytes in the FIFO
                if (n > backLen)
                {
                    return StatusCode.STATUS_NO_ROOM;
                }
                backLen = n;
                backData = new byte[backLen];// Number of bytes returned
                backData = PCD_ReadRegister((byte)PCD_Register.FIFODataReg, n, rxAlign);    // Get received data from FIFO
                if (backData == null) return StatusCode.STATUS_ERROR;
                _validBits = (byte)(PCD_ReadRegister((byte)PCD_Register.ControlReg) & 0x07);       // RxLastBits[2:0] indicates the number of valid bits in the last received byte. If this value is 000b, the whole byte is valid.
                if (validBits > 0)
                {
                    validBits = _validBits;
                }
            }

            // Tell about collisions
            if ((errorRegValue & 0x08) != 0)
            {       // CollErr
                return StatusCode.STATUS_COLLISION;
            }

            // Perform CRC_A validation if requested.
            if (backData.Length > 0 && backLen != 0 && checkCRC)
            {
                // In this case a MIFARE Classic NAK is not OK.
                if (backLen == 1 && _validBits == 4)
                {
                    return StatusCode.STATUS_MIFARE_NACK;
                }
                // We need at least the CRC_A value and all 8 bits of the last byte must be received.
                if (backLen < 2 || _validBits != 0)
                {
                    return StatusCode.STATUS_CRC_WRONG;
                }
                // Verify CRC_A - do our own calculation and store the control in controlBuffer.
                byte[] controlBuffer = new byte[2];
                StatusCode status = PCD_CalculateCRC(backData, (byte)(backLen - 2), out controlBuffer);
                if (status != StatusCode.STATUS_OK)
                {
                    return status;
                }
                if ((backData[backLen - 2] != controlBuffer[0]) || (backData[backLen - 1] != controlBuffer[1]))
                {
                    return StatusCode.STATUS_CRC_WRONG;
                }
            }

            return StatusCode.STATUS_OK;
        }

        //**** global output: backData, backLen, validBits
        private StatusCode PICC_RequestA(out byte[] bufferATQA,	///< The buffer to store the ATQA (Answer to request) in
											ref byte bufferSize	///< Buffer size, at least two bytes. Also number of bytes returned if STATUS_OK.
										)
        {
            return PICC_REQA_or_WUPA((byte)PICC_Command.PICC_CMD_REQA, out bufferATQA, ref bufferSize);
        }

        //**** global output: backData, backLen, validBits
        private StatusCode PICC_WakeupA(out byte[] bufferATQA,	///< The buffer to store the ATQA (Answer to request) in
											ref byte bufferSize	///< Buffer size, at least two bytes. Also number of bytes returned if STATUS_OK.                                            
                                        )
        {
            return PICC_REQA_or_WUPA((byte)PICC_Command.PICC_CMD_WUPA, out bufferATQA, ref bufferSize);
        }

        //**** global output: backData, backLen, validBits
        private StatusCode PICC_REQA_or_WUPA(byte command, 		///< The command to send - PICC_CMD_REQA or PICC_CMD_WUPA
												out byte[] bufferATQA,	///< The buffer to store the ATQA (Answer to request) in
												ref byte bufferSize ///< Buffer size, at least two bytes. Also number of bytes returned if STATUS_OK.

                                            )
        {
            byte validBits;
            StatusCode status;
            bufferATQA = new byte[bufferSize];

            if (bufferSize < 2)
            {   // The ATQA response is 2 bytes long.
                return StatusCode.STATUS_NO_ROOM;
            }
            PCD_ClearRegisterBitMask((byte)PCD_Register.CollReg, 0x80);        // ValuesAfterColl=1 => Bits received after collision are cleared.
            validBits = 7;                                  // For REQA and WUPA we need the short frame format - transmit only 7 bits of the last (and only) byte. TxLastBits = BitFramingReg[2..0]
            status = PCD_TransceiveData(new byte[] { command }, 1, out bufferATQA, ref bufferSize, ref validBits);
            if (status != StatusCode.STATUS_OK)
            {
                return status;
            }
            if (bufferSize != 2 || validBits != 0)
            {       // ATQA must be exactly 16 bits.
                return StatusCode.STATUS_ERROR;
            }
            return StatusCode.STATUS_OK;
        }

        // output: uid
        private StatusCode PICC_Select(ref Uid uid,			///< Pointer to Uid struct. Normally output, but can also be used to supply a known UID.
											byte validBits = 0		///< The number of known UID bits supplied in *uid. Normally 0. If set you must also supply uid->size.
										 )
        {
            bool uidComplete;
            bool selectDone;
            bool useCascadeTag;
            byte cascadeLevel = 1;
            StatusCode result;
            byte count;
            byte checkBit;
            byte index;
            byte uidIndex;                  // The first index in uid->uidByte[] that is used in the current Cascade Level.
            int currentLevelKnownBits;       // The number of known UID bits in the current Cascade Level.
            byte[] buffer = new byte[9];                 // The SELECT/ANTICOLLISION commands uses a 7 byte standard frame + 2 bytes CRC_A
            byte bufferUsed;                // The number of bytes used in the buffer, ie the number of bytes to transfer to the FIFO.
            byte rxAlign;                   // Used in BitFramingReg. Defines the bit position for the first bit received.
            byte txLastBits = 0;                // Used in BitFramingReg. The number of valid bits in the last transmitted byte. 
            byte[] responseBuffer = new byte[1];
            byte responseLength = 0;

            /*
             Description of buffer structure:
            		Byte 0: SEL 				Indicates the Cascade Level: PICC_CMD_SEL_CL1, PICC_CMD_SEL_CL2 or PICC_CMD_SEL_CL3
            		Byte 1: NVB					Number of Valid Bits (in complete command, not just the UID): High nibble: complete bytes, Low nibble: Extra bits. 
            		Byte 2: UID-data or CT		See explanation below. CT means Cascade Tag.
            		Byte 3: UID-data
            		Byte 4: UID-data
            		Byte 5: UID-data
            		Byte 6: BCC					Block Check Character - XOR of bytes 2-5
            		Byte 7: CRC_A
            		Byte 8: CRC_A
             The BCC and CRC_A are only transmitted if we know all the UID bits of the current Cascade Level.
            
             Description of bytes 2-5: (Section 6.5.4 of the ISO/IEC 14443-3 draft: UID contents and cascade levels)
            		UID size	Cascade level	Byte2	Byte3	Byte4	Byte5
            		========	=============	=====	=====	=====	=====
            		 4 bytes		1			uid0	uid1	uid2	uid3

            		 7 bytes		1			CT		uid0	uid1	uid2
            						2			uid3	uid4	uid5	uid6

            		10 bytes		1			CT		uid0	uid1	uid2
            						2			CT		uid3	uid4	uid5
            						3			uid6	uid7	uid8	uid9
            */

            // Sanity checks
            if (validBits > 80)
            {
                return StatusCode.STATUS_INVALID;
            }

            // Prepare MFRC522
            PCD_ClearRegisterBitMask((byte)PCD_Register.CollReg, 0x80); // ValuesAfterColl=1 => Bits received after collision are cleared.

            // Repeat Cascade Level loop until we have a complete UID.
            uidComplete = false;
            while (!uidComplete)
            {
                // Set the Cascade Level in the SEL byte, find out if we need to use the Cascade Tag in byte 2.
                switch (cascadeLevel)
                {
                    case 1:
                        buffer[0] = (byte)PICC_Command.PICC_CMD_SEL_CL1;
                        uidIndex = 0;
                        if (validBits > 0 && uid.size > 4) useCascadeTag = true; // When we know that the UID has more than 4 bytes
                        else useCascadeTag = false;
                        break;

                    case 2:
                        buffer[0] = (byte)PICC_Command.PICC_CMD_SEL_CL2;
                        uidIndex = 3;
                        if (validBits > 0 && uid.size > 7) useCascadeTag = true; // When we know that the UID has more than 7 bytes
                        else useCascadeTag = false;
                        break;

                    case 3:
                        buffer[0] = (byte)PICC_Command.PICC_CMD_SEL_CL3;
                        uidIndex = 6;
                        useCascadeTag = false;                      // Never used in CL3.
                        break;

                    default:
                        return StatusCode.STATUS_INTERNAL_ERROR;
                }

                // How many UID bits are known in this Cascade Level?
                currentLevelKnownBits = (validBits - (8 * uidIndex));
                if (currentLevelKnownBits < 0)
                {
                    currentLevelKnownBits = 0;
                }
                // Copy the known bits from uid->uidByte[] to buffer[]
                index = 2; // destination index in buffer[]
                if (useCascadeTag)
                {
                    buffer[index++] = (byte)PICC_Command.PICC_CMD_CT;
                }
                byte tmp = 0;
                if ((currentLevelKnownBits % 8) != 0) tmp = 1;
                byte bytesToCopy = (byte)(currentLevelKnownBits / 8 + tmp); // The number of bytes needed to represent the known bits for this level.
                if (bytesToCopy > 0)
                {
                    byte maxBytes = useCascadeTag ? (byte)3 : (byte)4; // Max 4 bytes in each Cascade Level. Only 3 left if we use the Cascade Tag
                    if (bytesToCopy > maxBytes)
                    {
                        bytesToCopy = maxBytes;
                    }
                    for (count = 0; count < bytesToCopy; count++)
                    {
                        buffer[index] = uid.uidByte[uidIndex + count];
                        index++;
                    }
                }
                // Now that the data has been copied we need to include the 8 bits in CT in currentLevelKnownBits
                if (useCascadeTag)
                {
                    currentLevelKnownBits += 8;
                }

                // Repeat anti collision loop until we can transmit all UID bits + BCC and receive a SAK - max 32 iterations.
                selectDone = false;
                while (!selectDone)
                {
                    // Find out how many bits and bytes to send and receive.
                    if (currentLevelKnownBits >= 32)
                    { // All UID bits in this Cascade Level are known. This is a SELECT.
                      //Serial.print(F("SELECT: currentLevelKnownBits=")); Serial.println(currentLevelKnownBits, DEC);
                        buffer[1] = 0x70; // NVB - Number of Valid Bits: Seven whole bytes
                                          // Calculate BCC - Block Check Character
                        buffer[6] = (byte)(buffer[2] ^ buffer[3] ^ buffer[4] ^ buffer[5]);
                        // Calculate CRC_A
                        byte[] crc1 = new byte[2];
                        result = PCD_CalculateCRC(buffer, 7, out crc1);
                        buffer[7] = crc1[0];
                        buffer[8] = crc1[1];
                        if (result != StatusCode.STATUS_OK)
                        {
                            return result;
                        }
                        txLastBits = 0; // 0 => All 8 bits are valid.
                        bufferUsed = 9;
                        // Store response in the last 3 bytes of buffer (BCC and CRC_A - not needed after tx)
                        //responseBuffer = &buffer[6];                        
                        responseLength = 3;
                        responseBuffer = new byte[responseLength];
                        //for (int i = 0; i < responseLength; i++) responseBuffer[i] = buffer[6 + i];
                    }
                    else
                    { // This is an ANTICOLLISION.
                      //Serial.print(F("ANTICOLLISION: currentLevelKnownBits=")); Serial.println(currentLevelKnownBits, DEC);
                        txLastBits = (byte)(currentLevelKnownBits % 8);
                        count = (byte)(currentLevelKnownBits / 8);  // Number of whole bytes in the UID part.
                        index = (byte)(2 + count);                  // Number of whole bytes: SEL + NVB + UIDs
                        buffer[1] = (byte)((index << 4) + txLastBits);  // NVB - Number of Valid Bits
                        byte tmp1 = 0;

                        //bufferUsed = index + (txLastBits ? 1 : 0);
                        if (txLastBits != 0) tmp1 = 1;
                        bufferUsed = (byte)(index + tmp1);
                        // Store response in the unused part of buffer
                        //responseBuffer = &buffer[index];
                        responseLength = (byte)(buffer.Length - index);
                        responseBuffer = new byte[responseLength];
                        //for (int i = 0; i < responseLength; i++) responseBuffer[i] = buffer[index + i];
                    }

                    // Set bit adjustments
                    rxAlign = txLastBits; // Having a separate variable is overkill. But it makes the next line easier to read.
                    PCD_WriteRegister((byte)PCD_Register.BitFramingReg, (byte)((rxAlign << 4) + txLastBits)); // RxAlign = BitFramingReg[6..4]. TxLastBits = BitFramingReg[2..0]

                    // Transmit the buffer and receive the response.
                    result = PCD_TransceiveData(buffer, bufferUsed, out responseBuffer, ref responseLength, ref txLastBits, rxAlign);

                    /*if (buffer[0] == 147 && buffer[1] == 32 && responseBuffer.Length == 5)
                    {
                        responseBuffer = MFRC522_Anticoll();
                        result = StatusCode.STATUS_OK;
                    }*/

                    if (currentLevelKnownBits >= 32)
                    {
                        for (int i = 0; i < responseLength; i++) buffer[6 + i] = responseBuffer[i];
                    }
                    else
                    {
                        for (int i = 0; i < responseLength; i++) buffer[index + i] = responseBuffer[i];
                    }


                    if (result == StatusCode.STATUS_COLLISION)
                    { // More than one PICC in the field => collision.
                        byte valueOfCollReg = PCD_ReadRegister((byte)PCD_Register.CollReg); // CollReg[7..0] bits are: ValuesAfterColl reserved CollPosNotValid CollPos[4:0]
                        if ((valueOfCollReg & 0x20) != 0)
                        { // CollPosNotValid
                            return StatusCode.STATUS_COLLISION; // Without a valid collision position we cannot continue
                        }
                        byte collisionPos = (byte)(valueOfCollReg & 0x1F); // Values 0-31, 0 means bit 32.
                        if (collisionPos == 0)
                        {
                            collisionPos = 32;
                        }
                        if (collisionPos <= currentLevelKnownBits)
                        { // No progress - should not happen 
                            return StatusCode.STATUS_INTERNAL_ERROR;
                        }
                        // Choose the PICC with the bit set.
                        currentLevelKnownBits = collisionPos;
                        count = (byte)(currentLevelKnownBits % 8); // The bit to modify
                        checkBit = (byte)((currentLevelKnownBits - 1) % 8);
                        byte tmp1 = 0;
                        if (count != 0) tmp1 = 1;
                        index = (byte)(1 + (currentLevelKnownBits / 8) + tmp1); // First byte is index 0.
                        buffer[index] |= (byte)(1 << count);
                    }
                    else if (result != StatusCode.STATUS_OK)
                    {
                        return result;
                    }
                    else
                    { // STATUS_OK
                        if (currentLevelKnownBits >= 32)
                        { // This was a SELECT.
                            selectDone = true; // No more anticollision 
                                               // We continue below outside the while.
                        }
                        else
                        { // This was an ANTICOLLISION.
                          // We now have all 32 bits of the UID in this Cascade Level
                            currentLevelKnownBits = 32;
                            // Run loop again to do the SELECT.
                        }
                    }
                } // End of while (!selectDone)

                // We do not check the CBB - it was constructed by us above.

                // Copy the found UID bytes from buffer[] to uid->uidByte[]
                index = (buffer[2] == (byte)PICC_Command.PICC_CMD_CT) ? (byte)3 : (byte)2; // source index in buffer[]
                bytesToCopy = (buffer[2] == (byte)PICC_Command.PICC_CMD_CT) ? (byte)3 : (byte)4;

                for (count = 0; count < bytesToCopy; count++)
                {
                    uid.uidByte[uidIndex + count] = buffer[index++];
                }

                // Check response SAK (Select Acknowledge)
                if (responseLength != 3 || txLastBits != 0) // SAK must be exactly 24 bits (1 byte + CRC_A).
                {
                    return StatusCode.STATUS_ERROR;
                }
                // Verify CRC_A - do our own calculation and store the control in buffer[2..3] - those bytes are not needed anymore.
                byte[] crc = new byte[2];
                result = PCD_CalculateCRC(responseBuffer, 1, out crc);
                buffer[2] = crc[0];
                buffer[3] = crc[1];
                if (result != StatusCode.STATUS_OK)
                {
                    return result;
                }
                if ((buffer[2] != responseBuffer[1]) || (buffer[3] != responseBuffer[2]))
                {
                    return StatusCode.STATUS_CRC_WRONG;
                }
                if ((responseBuffer[0] & 0x04) != 0)
                { // Cascade bit set - UID not complete yes
                    cascadeLevel++;
                }
                else
                {
                    uidComplete = true;
                    uid.sak = responseBuffer[0];
                }
            } // End of while (!uidComplete)

            // Set correct uid->size
            uid.size = (byte)(3 * cascadeLevel + 1);

            return StatusCode.STATUS_OK;
        }

        private StatusCode PICC_HaltA()
        {
            StatusCode result;
            byte[] buffer = new byte[4];

            // Build command buffer
            buffer[0] = (byte)PICC_Command.PICC_CMD_HLTA;
            buffer[1] = 0;
            // Calculate CRC_A
            byte[] crc = new byte[2];
            result = PCD_CalculateCRC(buffer, 2, out crc);
            buffer[2] = crc[0];
            buffer[3] = crc[1];

            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }

            // Send the command.
            // The standard says:
            //		If the PICC responds with any modulation during a period of 1 ms after the end of the frame containing the
            //		HLTA command, this response shall be interpreted as 'not acknowledge'.
            // We interpret that this way: Only STATUS_TIMEOUT is a success.
            byte[] tmp = new byte[0];
            byte t1 = 0;
            byte t2 = 0;
            result = PCD_TransceiveData(buffer, (byte)buffer.Length, out tmp, ref t1, ref t2);
            if (result == StatusCode.STATUS_TIMEOUT)
            {
                return StatusCode.STATUS_OK;
            }
            if (result == StatusCode.STATUS_OK)
            { // That is ironically NOT ok in this case ;-)
                return StatusCode.STATUS_ERROR;
            }
            return result;
        }

        #endregion

        #region communicating with MIFARE PICCs

        public StatusCode PCD_Authenticate(byte command,     ///< PICC_CMD_MF_AUTH_KEY_A or PICC_CMD_MF_AUTH_KEY_B
                                            byte blockAddr,     ///< The block number. See numbering in the comments in the .h file.
                                            byte[] key,    ///< Pointer to the Crypteo1 key to use (6 bytes)
                                            Uid uid            ///< Pointer to Uid struct. The first 4 bytes of the UID is used.
                                            )
        {
            byte waitIRq = 0x10;        // IdleIRq

            // Build command buffer
            byte[] sendData = new byte[12];
            sendData[0] = command;
            sendData[1] = blockAddr;
            for (byte i = 0; i < (byte)MIFARE_Misc.MF_KEY_SIZE; i++)
            {   // 6 key bytes
                sendData[2 + i] = key[i];
            }
            // Use the last uid bytes as specified in http://cache.nxp.com/documents/application_note/AN10927.pdf
            // section 3.2.5 "MIFARE Classic Authentication".
            // The only missed case is the MF1Sxxxx shortcut activation,
            // but it requires cascade tag (CT) byte, that is not part of uid.
            for (byte i = 0; i < 4; i++)
            {               // The last 4 bytes of the UID
                sendData[8 + i] = uid.uidByte[i + uid.size - 4];
            }

            // Start the authentication.
            byte[] tmp = new byte[0];
            byte tmp1 = 0;
            byte tmp2 = 0;
            return PCD_CommunicateWithPICC((byte)PCD_Command.PCD_MFAuthent, waitIRq, sendData, (byte)sendData.Length, out tmp, ref tmp1, ref tmp2);
        }

        // Used to exit the PCD from its authenticated state.
        public void PCD_StopCrypto1()
        {
            // Clear MFCrypto1On bit
            PCD_ClearRegisterBitMask((byte)PCD_Register.Status2Reg, 0x08); // Status2Reg[7..0] bits are: TempSensClear I2CForceHS reserved reserved MFCrypto1On ModemState[2:0]
        }

        public StatusCode MIFARE_Read(byte blockAddr, 	///< MIFARE Classic: The block (0-0xff) number. MIFARE Ultralight: The first page to return data from.
											out byte[] buffer,		///< The buffer to store the data in
											byte bufferSize	///< Buffer size, at least 18 bytes. Also number of bytes returned if STATUS_OK.
										)
        {
            StatusCode result;
            buffer = new byte[bufferSize];

            // Sanity check
            if (buffer == null || bufferSize < 18)
            {
                return StatusCode.STATUS_NO_ROOM;
            }

            // Build command buffer
            buffer[0] = (byte)PICC_Command.PICC_CMD_MF_READ;
            buffer[1] = blockAddr;
            // Calculate CRC_A
            byte[] crc = new byte[2];
            result = PCD_CalculateCRC(buffer, 2, out crc);
            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }
            buffer[2] = crc[0];
            buffer[3] = crc[1];

            // Transmit the buffer and receive the response, validate CRC_A.
            byte t = 0;
            return PCD_TransceiveData(buffer, 4, out buffer, ref bufferSize, ref t, 0, true);
        }

        public StatusCode MIFARE_Write(byte blockAddr, ///< MIFARE Classic: The block (0-0xff) number. MIFARE Ultralight: The page (2-15) to write to.
                                          byte[] buffer, ///< The 16 bytes to write to the PICC
                                            byte bufferSize ///< Buffer size, must be at least 16 bytes. Exactly 16 bytes are written.
                                        )
        {
            StatusCode result;

            // Sanity check
            if (buffer == null || bufferSize < 16)
            {
                return StatusCode.STATUS_INVALID;
            }

            // Mifare Classic protocol requires two communications to perform a write.
            // Step 1: Tell the PICC we want to write to block blockAddr.
            byte[] cmdBuffer = new byte[2];
            cmdBuffer[0] = (byte)PICC_Command.PICC_CMD_MF_WRITE;
            cmdBuffer[1] = blockAddr;
            result = PCD_MIFARE_Transceive(cmdBuffer, 2); // Adds CRC_A and checks that the response is MF_ACK.
            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }
            // Step 2: Transfer the data
            result = PCD_MIFARE_Transceive(buffer, bufferSize); // Adds CRC_A and checks that the response is MF_ACK.
            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }

            return StatusCode.STATUS_OK;
        }

        public StatusCode MIFARE_Ultralight_Write(byte page,         ///< The page (2-15) to write to.
                                                        byte[] buffer,   ///< The 4 bytes to write to the PICC
                                                        byte bufferSize ///< Buffer size, must be at least 4 bytes. Exactly 4 bytes are written.
                                                    )
        {
            StatusCode result = StatusCode.STATUS_INVALID;

            // Sanity check
            if (buffer == null || bufferSize < 4)
            {
                return result;
            }

            // Build commmand buffer
            byte[] cmdBuffer = new byte[6];
            cmdBuffer[0] = (byte)PICC_Command.PICC_CMD_UL_WRITE;
            cmdBuffer[1] = page;
            //memcpy(&cmdBuffer[2], buffer, 4);
            cmdBuffer[2] = buffer[0];
            cmdBuffer[3] = buffer[1];
            cmdBuffer[4] = buffer[2];
            cmdBuffer[5] = buffer[3];

            // Perform the write
            result = PCD_MIFARE_Transceive(cmdBuffer, 6); // Adds CRC_A and checks that the response is MF_ACK.
            return result;
        }

        public StatusCode MIFARE_Decrement(byte blockAddr, ///< The block (0-0xff) number.
                                              int delta     ///< This number is subtracted from the value of block blockAddr.
                                            )
        {
            return MIFARE_TwoStepHelper((byte)PICC_Command.PICC_CMD_MF_DECREMENT, blockAddr, delta);
        }

        public StatusCode MIFARE_Increment(byte blockAddr, ///< The block (0-0xff) number.
                                              int delta     ///< This number is added to the value of block blockAddr.
                                            )
        {
            return MIFARE_TwoStepHelper((byte)PICC_Command.PICC_CMD_MF_INCREMENT, blockAddr, delta);
        }

        public StatusCode MIFARE_Restore(byte blockAddr ///< The block (0-0xff) number.
                                         )
        {
            // The datasheet describes Restore as a two step operation, but does not explain what data to transfer in step 2.
            // Doing only a single step does not work, so I chose to transfer 0L in step two.
            return MIFARE_TwoStepHelper((byte)PICC_Command.PICC_CMD_MF_RESTORE, blockAddr, 0);
        }

        private StatusCode MIFARE_TwoStepHelper(byte command, ///< The command to use
                                                    byte blockAddr, ///< The block (0-0xff) number.
                                                    int data        ///< The data to transfer in step 2
                                                    )
        {
            StatusCode result;
            byte[] cmdBuffer = new byte[2]; // We only need room for 2 bytes.

            // Step 1: Tell the PICC the command and block address
            cmdBuffer[0] = command;
            cmdBuffer[1] = blockAddr;
            result = PCD_MIFARE_Transceive(cmdBuffer, 2); // Adds CRC_A and checks that the response is MF_ACK.
            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }

            // Step 2: Transfer the data
            result = PCD_MIFARE_Transceive(new byte[] { (byte)data }, 4, true); // Adds CRC_A and accept timeout as success.
            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }

            return StatusCode.STATUS_OK;
        }

        private StatusCode MIFARE_Transfer(byte blockAddr ///< The block (0-0xff) number.
                                            )
        {
            StatusCode result;
            byte[] cmdBuffer = new byte[2]; // We only need room for 2 bytes.

            // Tell the PICC we want to transfer the result into block blockAddr.
            cmdBuffer[0] = (byte)PICC_Command.PICC_CMD_MF_TRANSFER;
            cmdBuffer[1] = blockAddr;
            result = PCD_MIFARE_Transceive(cmdBuffer, 2); // Adds CRC_A and checks that the response is MF_ACK.
            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }
            return StatusCode.STATUS_OK;
        }

        public StatusCode MIFARE_GetValue(byte blockAddr, int value)
        {
            StatusCode status;
            byte[] buffer = new byte[18];
            byte size = (byte)buffer.Length;

            // Read the block
            status = MIFARE_Read(blockAddr, out buffer, size);
            if (status == StatusCode.STATUS_OK)
            {
                // Extract the value
                value = ((int)(buffer[3]) << 24) | ((int)(buffer[2]) << 16) | ((int)(buffer[1]) << 8) | (int)(buffer[0]);
            }
            return status;
        }

        public StatusCode MIFARE_SetValue(byte blockAddr, int value)
        {
            byte[] buffer = new byte[18];

            // Translate the int32_t into 4 bytes; repeated 2x in value block
            buffer[0] = buffer[8] = (byte)(value & 0xFF);
            buffer[1] = buffer[9] = (byte)((value & 0xFF00) >> 8);
            buffer[2] = buffer[10] = (byte)((value & 0xFF0000) >> 16);
            buffer[3] = buffer[11] = (byte)((value & 0xFF000000) >> 24);
            // Inverse 4 bytes also found in value block
            buffer[4] = (byte)(~buffer[0]);
            buffer[5] = (byte)(~buffer[1]);
            buffer[6] = (byte)(~buffer[2]);
            buffer[7] = (byte)(~buffer[3]);
            // Address 2x with inverse address 2x
            buffer[12] = buffer[14] = blockAddr;
            buffer[13] = buffer[15] = (byte)(~blockAddr);

            // Write the whole data block
            return MIFARE_Write(blockAddr, buffer, 16);
        }

        public StatusCode PCD_NTAG216_AUTH(byte[] passWord, out byte[] pACK) //Authenticate with 32bit password
        {
            // TODO: Fix cmdBuffer length and rxlength. They really should match.
            //       (Better still, rxlength should not even be necessary.)
            pACK = new byte[2];

            StatusCode result;
            byte[] cmdBuffer = new byte[7]; // We need room for 5 bytes data and 2 bytes CRC_A.

            cmdBuffer[0] = 0x1B; //Comando de autentificacion

            if (passWord.Length != 4) return StatusCode.STATUS_ERROR;
            for (byte i = 0; i < 4; i++)
                cmdBuffer[i + 1] = passWord[i];

            byte[] crc = new byte[2];
            result = PCD_CalculateCRC(cmdBuffer, 5, out crc);
            cmdBuffer[5] = crc[0];
            cmdBuffer[6] = crc[1];

            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }

            // Transceive the data, store the reply in cmdBuffer[]
            byte waitIRq = 0x30;    // RxIRq and IdleIRq
                                    //	byte cmdBufferSize	= sizeof(cmdBuffer);
            byte validBits = 0;
            byte rxlength = 5;
            result = PCD_CommunicateWithPICC((byte)PCD_Command.PCD_Transceive, waitIRq, cmdBuffer, 7, out cmdBuffer, ref rxlength, ref validBits);
            pACK[0] = cmdBuffer[0];
            pACK[1] = cmdBuffer[1];
            return result;
        }

        #endregion

        #region Support functions

        private StatusCode PCD_MIFARE_Transceive(byte[] sendData,      ///< Pointer to the data to transfer to the FIFO. Do NOT include the CRC_A.
                                                    byte sendLen,       ///< Number of bytes in sendData.
                                                    bool acceptTimeout = false  ///< True => A timeout is also success
                                                )
        {
            StatusCode result;

            // Sanity check
            if (sendData == null || sendLen > 16)
            {
                return StatusCode.STATUS_INVALID;
            }

            byte[] cmdBuffer = new byte[18]; // We need room for 16 bytes data and 2 bytes CRC_A.
            for (int i = 0; i < sendLen; i++) cmdBuffer[i] = sendData[i];

            byte[] crcA = new byte[2];
            result = PCD_CalculateCRC(cmdBuffer, sendLen, out crcA);
            // Copy sendData[] to cmdBuffer[] and add CRC_A
            cmdBuffer[sendLen] = crcA[0];
            cmdBuffer[sendLen + 1] = crcA[1];
            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }
            sendLen += 2;

            // Transceive the data, store the reply in cmdBuffer[]
            byte waitIRq = 0x30;        // RxIRq and IdleIRq
            byte cmdBufferSize = (byte)cmdBuffer.Length;
            byte validBits = 0;
            result = PCD_CommunicateWithPICC((byte)PCD_Command.PCD_Transceive, waitIRq, cmdBuffer, sendLen, out cmdBuffer, ref cmdBufferSize, ref validBits);
            if (acceptTimeout && result == StatusCode.STATUS_TIMEOUT)
            {
                return StatusCode.STATUS_OK;
            }
            if (result != StatusCode.STATUS_OK)
            {
                return result;
            }
            // The PICC must reply with a 4 bit ACK
            if (cmdBufferSize != 1 || validBits != 4)
            {
                //return StatusCode.STATUS_ERROR;
            }
            if (cmdBuffer[0] != (byte)MIFARE_Misc.MF_ACK)
            {
                return StatusCode.STATUS_MIFARE_NACK;
            }
            return StatusCode.STATUS_OK;
        }

        private string GetStatusCodeName(StatusCode code)  ///< One of the StatusCode enums.                                        
        {
            switch (code)
            {
                case StatusCode.STATUS_OK: return "Success.";
                case StatusCode.STATUS_ERROR: return "Error in communication.";
                case StatusCode.STATUS_COLLISION: return "Collission detected.";
                case StatusCode.STATUS_TIMEOUT: return "Timeout in communication.";
                case StatusCode.STATUS_NO_ROOM: return "A buffer is not big enough.";
                case StatusCode.STATUS_INTERNAL_ERROR: return "Internal error in the code. Should not happen.";
                case StatusCode.STATUS_INVALID: return "Invalid argument.";
                case StatusCode.STATUS_CRC_WRONG: return "The CRC_A does not match.";
                case StatusCode.STATUS_MIFARE_NACK: return "A MIFARE PICC responded with NAK.";
                default: return "Unknown error";
            }
        }

        public PICC_Type PICC_GetType(byte sak)		///< The SAK byte returned from PICC_Select().
        {
            // http://www.nxp.com/documents/application_note/AN10833.pdf 
            // 3.2 Coding of Select Acknowledge (SAK)
            // ignore 8-bit (iso14443 starts with LSBit = bit 1)
            // fixes wrong type for manufacturer Infineon (http://nfc-tools.org/index.php?title=ISO14443A)
            sak &= 0x7F;
            switch (sak)
            {
                case 0x04: return PICC_Type.PICC_TYPE_NOT_COMPLETE;   // UID not complete
                case 0x09: return PICC_Type.PICC_TYPE_MIFARE_MINI;
                case 0x08: return PICC_Type.PICC_TYPE_MIFARE_1K;
                case 0x18: return PICC_Type.PICC_TYPE_MIFARE_4K;
                case 0x00: return PICC_Type.PICC_TYPE_MIFARE_UL;
                case 0x10:
                case 0x11: return PICC_Type.PICC_TYPE_MIFARE_PLUS;
                case 0x01: return PICC_Type.PICC_TYPE_TNP3XXX;
                case 0x20: return PICC_Type.PICC_TYPE_ISO_14443_4;
                case 0x40: return PICC_Type.PICC_TYPE_ISO_18092;
                default: return PICC_Type.PICC_TYPE_UNKNOWN;
            }
        }

        public string PICC_GetTypeName(PICC_Type piccType) ///< One of the PICC_Type enums.                                                    )
        {
            switch (piccType)
            {
                case PICC_Type.PICC_TYPE_ISO_14443_4: return "PICC compliant with ISO/IEC 14443-4";
                case PICC_Type.PICC_TYPE_ISO_18092: return "PICC compliant with ISO/IEC 18092 (NFC)";
                case PICC_Type.PICC_TYPE_MIFARE_MINI: return "MIFARE Mini, 320 bytes";
                case PICC_Type.PICC_TYPE_MIFARE_1K: return "MIFARE Classic 1KB";
                case PICC_Type.PICC_TYPE_MIFARE_4K: return "MIFARE Classic 4KB";
                case PICC_Type.PICC_TYPE_MIFARE_UL: return "MIFARE Ultralight or Ultralight C";
                case PICC_Type.PICC_TYPE_MIFARE_PLUS: return "MIFARE Plus";
                case PICC_Type.PICC_TYPE_MIFARE_DESFIRE: return "MIFARE DESFire";
                case PICC_Type.PICC_TYPE_TNP3XXX: return "MIFARE TNP3XXX";
                case PICC_Type.PICC_TYPE_NOT_COMPLETE: return "SAK indicates UID is not complete.";
                case PICC_Type.PICC_TYPE_UNKNOWN:
                default: return "Unknown type";
            }
        }

        public string PCD_GetFwVersion()
        {
            string str;
            // Get the MFRC522 firmware version
            byte v = PCD_ReadRegister((byte)PCD_Register.VersionReg);
            str = "Firmware Version: 0x";
            str += v.ToString("X2");
            // Lookup which version
            switch (v)
            {
                case 0x88: str += " = (clone)\r\n"; break;
                case 0x90: str += " = v0.0\r\n"; break;
                case 0x91: str += " = v1.0\r\n"; break;
                case 0x92: str += " = v2.0\r\n"; break;
                default: str += " = (unknown)\r\n"; break;
            }
            // When 0x00 or 0xFF is returned, communication probably failed
            if ((v == 0x00) || (v == 0xFF))
                str += "WARNING: Communication failure, is the MFRC522 properly connected?\r\n";
            return str;
        }

        // Dumps memory contents of a sector of a MIFARE Classic PICC.
        // Uses PCD_Authenticate(), MIFARE_Read() and PCD_StopCrypto1.
        // Always uses PICC_CMD_MF_AUTH_KEY_A because only Key A can always read the sector trailer access bits.
        private byte[] PICC_DumpMifareClassicSector(Uid uid,            ///< Pointer to Uid struct returned from a successful PICC_Select().
                                                    byte[] key,    ///< Key A for the sector.
                                                    byte sector         ///< The sector to dump, 0..39.
                                                    )
        {
            StatusCode status;
            byte firstBlock;        // Address of lowest address to dump actually last block dumped)
            byte no_of_blocks;      // Number of blocks in sector
            bool isSectorTrailer;   // Set to true while handling the "last" (ie highest address) in the sector.

            // Determine position and size of sector.
            if (sector < 32)
            { // Sectors 0..31 has 4 blocks each
                no_of_blocks = 4;
                firstBlock = (byte)(sector * no_of_blocks);
            }
            else if (sector < 40)
            { // Sectors 32-39 has 16 blocks each
                no_of_blocks = 16;
                firstBlock = (byte)(128 + (sector - 32) * no_of_blocks);
            }
            else
            { // Illegal input, no MIFARE Classic PICC has more than 40 sectors.
                return null;
            }

            // Dump blocks, highest address first.
            byte byteCount;
            byte[] buffer = new byte[18];
            byte blockAddr;
            isSectorTrailer = true;
            for (byte blockOffset = (byte)(no_of_blocks - 1); blockOffset >= 0; blockOffset--)
            {
                blockAddr = (byte)(firstBlock + blockOffset);
                // Establish encrypted communications before reading the first block
                if (isSectorTrailer)
                {
                    status = PCD_Authenticate((byte)PICC_Command.PICC_CMD_MF_AUTH_KEY_A, firstBlock, key, uid);
                    if (status != StatusCode.STATUS_OK)
                    {
                        // PCD_Authenticate() failed
                        return null;
                    }
                }
                // Read block
                byteCount = (byte)buffer.Length;
                status = MIFARE_Read(blockAddr, out buffer, byteCount);
                if (status != StatusCode.STATUS_OK)
                {
                    // MIFARE_Read() failed
                    return null;
                }
                /*
                // Parse sector trailer data
                // The access bits are stored in a peculiar fashion.
                // There are four groups:
                //		g[3]	Access bits for the sector trailer, block 3 (for sectors 0-31) or block 15 (for sectors 32-39)
                //		g[2]	Access bits for block 2 (for sectors 0-31) or blocks 10-14 (for sectors 32-39)
                //		g[1]	Access bits for block 1 (for sectors 0-31) or blocks 5-9 (for sectors 32-39)
                //		g[0]	Access bits for block 0 (for sectors 0-31) or blocks 0-4 (for sectors 32-39)
                // Each group has access bits [C1 C2 C3]. In this code C1 is MSB and C3 is LSB.
                // The four CX bits are stored together in a nible cx and an inverted nible cx_.
                byte c1, c2, c3;        // Nibbles
                byte c1_, c2_, c3_;     // Inverted nibbles
                bool invertedError;     // True if one of the inverted nibbles did not match
                byte[] g = new byte[4];              // Access bits for each of the four groups.
                byte group;             // 0-3 - active group for access bits
                bool firstInGroup;      // True for the first block dumped in the group
                invertedError = false;  // Avoid "unused variable" warning.

                if (isSectorTrailer)
                {
                    c1 = (byte)(buffer[7] >> 4);
                    c2 = (byte)(buffer[8] & 0xF);
                    c3 = (byte)(buffer[8] >> 4);
                    c1_ = (byte)(buffer[6] & 0xF);
                    c2_ = (byte)(buffer[6] >> 4);
                    c3_ = (byte)(buffer[7] & 0xF);
                    invertedError = (c1 != (~c1_ & 0xF)) || (c2 != (~c2_ & 0xF)) || (c3 != (~c3_ & 0xF));
                    g[0] = (byte)(((c1 & 1) << 2) | ((c2 & 1) << 1) | ((c3 & 1) << 0));
                    g[1] = (byte)(((c1 & 2) << 1) | ((c2 & 2) << 0) | ((c3 & 2) >> 1));
                    g[2] = (byte)(((c1 & 4) << 0) | ((c2 & 4) >> 1) | ((c3 & 4) >> 2));
                    g[3] = (byte)(((c1 & 8) >> 1) | ((c2 & 8) >> 2) | ((c3 & 8) >> 3));
                    isSectorTrailer = false;
                }

                // Which access group is this block in?
                if (no_of_blocks == 4)
                {
                    group = blockOffset;
                    firstInGroup = true;
                }
                else
                {
                    group = (byte)(blockOffset / 5);
                    firstInGroup = (group == 3) || (group != (blockOffset + 1) / 5);
                }

                if (firstInGroup)
                {
                    // Print access bits
                    Serial.print(F(" [ "));
                    Serial.print((g[group] >> 2) & 1, DEC); Serial.print(F(" "));
                    Serial.print((g[group] >> 1) & 1, DEC); Serial.print(F(" "));
                    Serial.print((g[group] >> 0) & 1, DEC);
                    Serial.print(F(" ] "));
                    if (invertedError)
                    {
                        Serial.print(F(" Inverted access bits did not match! "));
                    }
                }

                if (group != 3 && (g[group] == 1 || g[group] == 6))
                { // Not a sector trailer, a value block
                    int32_t value = (int32_t(buffer[3]) << 24) | (int32_t(buffer[2]) << 16) | (int32_t(buffer[1]) << 8) | int32_t(buffer[0]);
                    Serial.print(F(" Value=0x")); Serial.print(value, HEX);
                    Serial.print(F(" Adr=0x")); Serial.print(buffer[12], HEX);
                }
                */
            }

            return buffer;
        }

        public List<byte[]> PICC_DumpMifareClassic(Uid uid,			///< Pointer to Uid struct returned from a successful PICC_Select().
												PICC_Type piccType,	///< One of the PICC_Type enums.
												byte[] key		///< Key A used for all sectors.
											)
        {
            List<byte[]> data = new List<byte[]>();
            byte no_of_sectors = 0;
            switch (piccType)
            {
                case PICC_Type.PICC_TYPE_MIFARE_MINI:
                    // Has 5 sectors * 4 blocks/sector * 16 bytes/block = 320 bytes.
                    no_of_sectors = 5;
                    break;

                case PICC_Type.PICC_TYPE_MIFARE_1K:
                    // Has 16 sectors * 4 blocks/sector * 16 bytes/block = 1024 bytes.
                    no_of_sectors = 16;
                    break;

                case PICC_Type.PICC_TYPE_MIFARE_4K:
                    // Has (32 sectors * 4 blocks/sector + 8 sectors * 16 blocks/sector) * 16 bytes/block = 4096 bytes.
                    no_of_sectors = 40;
                    break;

                default: // Should not happen. Ignore.
                    break;
            }

            // Dump sectors, highest address first.
            if (no_of_sectors > 0)
            {
                for (byte i = (byte)(no_of_sectors - 1); i >= 0; i--)
                {
                    data.Add(PICC_DumpMifareClassicSector(uid, key, i));
                }
            }
            PICC_HaltA(); // Halt the PICC before stopping the encrypted session.
            PCD_StopCrypto1();
            return data;
        }

        public List<byte[]> PICC_DumpMifareUltralight()
        {
            List<byte[]> data = new List<byte[]>();
            StatusCode status;
            byte byteCount;
            byte[] buffer = new byte[18];
            byte i;

            //Serial.println(F("Page  0  1  2  3"));
            // Try the mpages of the original Ultralight. Ultralight C has more pages.
            for (byte page = 0; page < 16; page += 4)
            { // Read returns data for 4 pages at a time.
              // Read pages
                byteCount = (byte)buffer.Length;
                status = MIFARE_Read(page, out buffer, byteCount);
                if (status != StatusCode.STATUS_OK)
                {
                    //Serial.print(F("MIFARE_Read() failed: "));
                    //Serial.println(GetStatusCodeName(status));
                    data.Add(null);
                    break;
                }
                // Dump data
                data.Add(buffer);
            }
            return data;
        }

        // Calculates the bit pattern needed for the specified access bits. In the [C1 C2 C3] tuples C1 is MSB (=4) and C3 is LSB (=1).        
        // Returns bytes 6, 7 and 8 in the sector trailer.
        public byte[] MIFARE_SetAccessBits(
                                    byte g0,                ///< Access bits [C1 C2 C3] for block 0 (for sectors 0-31) or blocks 0-4 (for sectors 32-39)
                                    byte g1,                ///< Access bits [C1 C2 C3] for block 1 (for sectors 0-31) or blocks 5-9 (for sectors 32-39)
                                    byte g2,                ///< Access bits [C1 C2 C3] for block 2 (for sectors 0-31) or blocks 10-14 (for sectors 32-39)
                                    byte g3                 ///< Access bits [C1 C2 C3] for the sector trailer, block 3 (for sectors 0-31) or block 15 (for sectors 32-39)
                                )
        {
            byte c1 = (byte)(((g3 & 4) << 1) | ((g2 & 4) << 0) | ((g1 & 4) >> 1) | ((g0 & 4) >> 2));
            byte c2 = (byte)(((g3 & 2) << 2) | ((g2 & 2) << 1) | ((g1 & 2) << 0) | ((g0 & 2) >> 1));
            byte c3 = (byte)(((g3 & 1) << 3) | ((g2 & 1) << 2) | ((g1 & 1) << 1) | ((g0 & 1) << 0));

            byte[] accessBitBuffer = new byte[3];
            accessBitBuffer[0] = (byte)((~c2 & 0xF) << 4 | (~c1 & 0xF));
            accessBitBuffer[1] = (byte)(c1 << 4 | (~c3 & 0xF));
            accessBitBuffer[2] = (byte)(c3 << 4 | c2);
            return accessBitBuffer;
        }


        /*
        * Performs the "magic sequence" needed to get Chinese UID changeable
        * Mifare cards to allow writing to sector 0, where the card UID is stored.
        *
        * Note that you do not need to have selected the card through REQA or WUPA,
        * this sequence works immediately when the card is in the reader vicinity.
        * This means you can use this method even on "bricked" cards that your reader does
        * not recognise anymore (see MFRC522::MIFARE_UnbrickUidSector).
        * 
        * Of course with non-bricked devices, you're free to select them before calling this function.
        */
        private bool MIFARE_OpenUidBackdoor()
        {
            // Magic sequence:
            // > 50 00 57 CD (HALT + CRC)
            // > 40 (7 bits only)
            // < A (4 bits only)
            // > 43
            // < A (4 bits only)
            // Then you can write to sector 0 without authenticating

            PICC_HaltA(); // 50 00 57 CD

            byte cmd = 0x40;
            byte validBits = 7; // Our command is only 7 bits. After receiving card response,
                                //this will contain amount of valid response bits.
            byte[] response = new byte[32]; // Card's response is written here
            byte received = 0;
            StatusCode status = PCD_TransceiveData(new byte[] { cmd }, 1, out response, ref received, ref validBits, 0, false); // 40
            if (status != StatusCode.STATUS_OK)
            {
                // Card did not respond to 0x40 after HALT command. Are you sure it is a UID changeable one?
                return false;
            }
            if (received != 1 || response[0] != 0x0A)
            {
                // Got bad response on backdoor 0x40 command
            }

            cmd = 0x43;
            validBits = 8;
            status = PCD_TransceiveData(new byte[] { cmd }, 1, out response, ref received, ref validBits, 0, false); // 43
            if (status != StatusCode.STATUS_OK)
            {
                // Error in communication at command 0x43, after successfully executing 0x40
                return false;
            }
            if (received != 1 || response[0] != 0x0A)
            {
                // Got bad response on backdoor 0x43 command
                return false;
            }

            // You can now write to sector 0 without authenticating!
            return true;
        }

        /*
        * Reads entire block 0, including all manufacturer data, and overwrites
        * that block with the new UID, a freshly calculated BCC, and the original
        * manufacturer data.
        *
        * It assumes a default KEY A of 0xFFFFFFFFFFFF.
        * Make sure to have selected the card before this function is called.
        */
        public bool MIFARE_SetUid(byte[] newUid, byte uidSize, bool logErrors)
        {

            // UID + BCC byte can not be larger than 16 together
            if (newUid == null || uidSize == 0 || uidSize > 15)
            {
                // New UID buffer empty, size 0, or size > 15 given
                return false;
            }

            // Authenticate for reading
            byte[] key = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            StatusCode status = PCD_Authenticate((byte)PICC_Command.PICC_CMD_MF_AUTH_KEY_A, 1, key, uid);
            if (status != StatusCode.STATUS_OK)
            {

                if (status == StatusCode.STATUS_TIMEOUT)
                {
                    // We get a read timeout if no card is selected yet, so let's select one

                    // Wake the card up again if sleeping
                    //			  byte atqa_answer[2];
                    //			  byte atqa_size = 2;
                    //			  PICC_WakeupA(atqa_answer, &atqa_size);

                    if (!PICC_IsNewCardPresent() || !PICC_ReadCardSerial())
                    {
                        // No card was previously selected, and none are available. Failed to set UID.
                        return false;
                    }

                    status = PCD_Authenticate((byte)PICC_Command.PICC_CMD_MF_AUTH_KEY_A, 1, key, uid);
                    if (status != StatusCode.STATUS_OK)
                    {
                        // We tried, time to give up
                        // Failed to authenticate to card for reading, could not set UID:
                        return false;
                    }
                }
                else
                {
                    // PCD_Authenticate() failed:
                    return false;
                }
            }

            // Read block 0
            byte[] block0_buffer = new byte[18];
            byte byteCount = (byte)block0_buffer.Length;
            status = MIFARE_Read(0, out block0_buffer, byteCount);
            if (status != StatusCode.STATUS_OK)
            {
                // MIFARE_Read() failed:
                // Are you sure your KEY A for sector 0 is 0xFFFFFFFFFFFF?
                return false;
            }

            // Write new UID to the data we just read, and calculate BCC byte
            byte bcc = 0;
            for (byte i = 0; i < uidSize; i++)
            {
                block0_buffer[i] = newUid[i];
                bcc ^= newUid[i];
            }

            // Write BCC byte to buffer
            block0_buffer[uidSize] = bcc;

            // Stop encrypted traffic so we can send raw bytes
            PCD_StopCrypto1();

            // Activate UID backdoor
            if (!MIFARE_OpenUidBackdoor())
            {
                // Activating the UID backdoor failed.
                return false;
            }

            // Write modified block 0 back to card
            status = MIFARE_Write((byte)0, block0_buffer, (byte)16);
            if (status != StatusCode.STATUS_OK)
            {
                // MIFARE_Write() failed
                return false;
            }

            // Wake the card up again
            byte[] atqa_answer = new byte[2];
            byte atqa_size = 2;
            PICC_WakeupA(out atqa_answer, ref atqa_size);

            return true;
        }

        // Resets entire sector 0 to zeroes, so the card can be read again by readers. 
        public bool MIFARE_UnbrickUidSector(bool logErrors)
        {
            MIFARE_OpenUidBackdoor();

            byte[] block0_buffer = { 0x01, 0x02, 0x03, 0x04, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            // Write modified block 0 back to card
            StatusCode status = MIFARE_Write(0, block0_buffer, 16);
            if (status != StatusCode.STATUS_OK)
            {
                return false;
            }
            return true;
        }

        public static string[] getComPorts()
        {
            return SerialPort.GetPortNames();
        }

        private bool serWrite(byte b)
        {
            if (serialPort1.IsOpen)
            {
                byte[] val = new byte[1];
                val[0] = b;
                try
                {
                    serialPort1.Write(val, 0, 1);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static void Delay_ms(long milisec)
        {
            DateTime start = DateTime.Now;

            while (DateTime.Now.Subtract(start).TotalMilliseconds < milisec)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(1);
            }
        }

        public bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }

        #endregion

        #region Convenience functions - does not add extra functionality

        public bool PICC_IsNewCardPresent()
        {
            byte[] bufferATQA = new byte[2];
            byte bufferSize = (byte)bufferATQA.Length;
            // Reset baud rates
            PCD_WriteRegister((byte)PCD_Register.TxModeReg, 0x00);
            PCD_WriteRegister((byte)PCD_Register.RxModeReg, 0x00);
            // Reset ModWidthReg
            PCD_WriteRegister((byte)PCD_Register.ModWidthReg, 0x26);
            StatusCode result = PICC_RequestA(out bufferATQA, ref bufferSize);
            return (result == StatusCode.STATUS_OK || result == StatusCode.STATUS_COLLISION);
        }

        public bool PICC_IsAnyCardPresent()
        {
            byte[] bufferATQA = new byte[2];
            byte bufferSize = (byte)bufferATQA.Length;
            // Reset baud rates
            bool s = PCD_WriteRegister((byte)PCD_Register.TxModeReg, 0x00);
            s = PCD_WriteRegister((byte)PCD_Register.RxModeReg, 0x00);
            // Reset ModWidthReg
            s = PCD_WriteRegister((byte)PCD_Register.ModWidthReg, 0x26);
            StatusCode result = PICC_WakeupA(out bufferATQA, ref bufferSize);
            return (result == StatusCode.STATUS_OK || result == StatusCode.STATUS_COLLISION);
        }

        public bool PICC_ReadCardSerial()
        {
            uid.uidByte = new byte[10];
            StatusCode result = PICC_Select(ref uid);
            if (result == StatusCode.STATUS_OK)
            {
                PCD_StopCrypto1();
                return true;
            }
            else return false;
        }

        #endregion

    }
}
