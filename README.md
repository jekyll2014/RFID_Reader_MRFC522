# RFID_Reader_MRFC522
Work with MIFARE CLASSIC (and ULTRALITE but read only) using USB-UART adapter. Cheapest RFID reader for PC.
Project contains MRFC522 class and an example application.
RC522 chip most probably can't write MIFARE ULTRALIGHT as I haven't found any working example yet...

MRFC522 library was ported from
https://github.com/miguelbalboa/rfid
Thanks a lot for a great job!

How to change RC522 from SPI to UART:
https://github.com/jekyll2014/RFID_Reader_MRFC522/blob/master/rfid_docs/MRFC522-RC522-UART-RFID.jpg

or

https://github.com/mfdogalindo/MFRC522-UART

or

https://igor-kochet.livejournal.com/267758.html

Connecting pin32 to GND is not necessary but better to do.
