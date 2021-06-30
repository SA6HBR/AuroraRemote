EESchema Schematic File Version 4
EELAYER 30 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title "Aurora-Remote & TNC - interface"
Date "2021-06-30"
Rev "0.05"
Comp "SA6HBR"
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L Device:R 330R
U 1 1 60736B6F
P 2250 5100
F 0 "330R" V 2043 5100 50  0001 C CNN
F 1 "330R" V 2134 5100 50  0000 C CNN
F 2 "" V 2180 5100 50  0001 C CNN
F 3 "~" H 2250 5100 50  0001 C CNN
	1    2250 5100
	0    1    1    0   
$EndComp
$Comp
L Device:R 1K
U 1 1 60737225
P 3650 4500
F 0 "1K" V 3443 4500 50  0001 C CNN
F 1 "1k" V 3534 4500 50  0000 C CNN
F 2 "" V 3580 4500 50  0001 C CNN
F 3 "~" H 3650 4500 50  0001 C CNN
	1    3650 4500
	0    1    1    0   
$EndComp
$Comp
L Device:R 330R
U 1 1 607379B4
P 2250 4500
F 0 "330R" V 2043 4500 50  0001 C CNN
F 1 "330R" V 2134 4500 50  0000 C CNN
F 2 "" V 2180 4500 50  0001 C CNN
F 3 "~" H 2250 4500 50  0001 C CNN
	1    2250 4500
	0    1    1    0   
$EndComp
$Comp
L Device:R 1k
U 1 1 60737F5F
P 4050 6150
F 0 "1k" V 3843 6150 50  0001 C CNN
F 1 "1k" V 3934 6150 50  0000 C CNN
F 2 "" V 3980 6150 50  0001 C CNN
F 3 "~" H 4050 6150 50  0001 C CNN
	1    4050 6150
	0    1    1    0   
$EndComp
$Comp
L Device:R 10k
U 1 1 607385E0
P 1850 6450
F 0 "10k" H 1780 6404 50  0001 R CNN
F 1 "10k" H 1780 6495 50  0000 R CNN
F 2 "" V 1780 6450 50  0001 C CNN
F 3 "~" H 1850 6450 50  0001 C CNN
	1    1850 6450
	-1   0    0    1   
$EndComp
$Comp
L Device:R 330R
U 1 1 60738B09
P 3650 6700
F 0 "330R" V 3443 6700 50  0001 C CNN
F 1 "330R" V 3534 6700 50  0000 C CNN
F 2 "" V 3580 6700 50  0001 C CNN
F 3 "~" H 3650 6700 50  0001 C CNN
	1    3650 6700
	0    1    1    0   
$EndComp
$Comp
L Isolator:PC817 U4
U 1 1 609704D5
P 2800 6800
F 0 "U4" H 2800 7125 50  0001 C CNN
F 1 "PC817" H 2800 7034 50  0000 C CNN
F 2 "Package_DIP:DIP-4_W7.62mm" H 2600 6600 50  0001 L CIN
F 3 "http://www.soselectronic.cz/a_info/resource/d/pc817.pdf" H 2800 6800 50  0001 L CNN
	1    2800 6800
	-1   0    0    -1  
$EndComp
$Comp
L Isolator:PC817 U3
U 1 1 609719CD
P 2800 6250
F 0 "U3" H 2800 6575 50  0001 C CNN
F 1 "PC817" H 2800 6484 50  0000 C CNN
F 2 "Package_DIP:DIP-4_W7.62mm" H 2600 6050 50  0001 L CIN
F 3 "http://www.soselectronic.cz/a_info/resource/d/pc817.pdf" H 2800 6250 50  0001 L CNN
	1    2800 6250
	1    0    0    -1  
$EndComp
$Comp
L Isolator:PC817 U2
U 1 1 60973763
P 2800 5200
F 0 "U2" H 2800 5525 50  0001 C CNN
F 1 "PC817" H 2800 5434 50  0000 C CNN
F 2 "Package_DIP:DIP-4_W7.62mm" H 2600 5000 50  0001 L CIN
F 3 "http://www.soselectronic.cz/a_info/resource/d/pc817.pdf" H 2800 5200 50  0001 L CNN
	1    2800 5200
	1    0    0    -1  
$EndComp
$Comp
L Isolator:PC817 U1
U 1 1 609A4C9F
P 2800 4600
F 0 "U1" H 2800 4925 50  0001 C CNN
F 1 "PC817" H 2800 4834 50  0000 C CNN
F 2 "Package_DIP:DIP-4_W7.62mm" H 2600 4400 50  0001 L CIN
F 3 "http://www.soselectronic.cz/a_info/resource/d/pc817.pdf" H 2800 4600 50  0001 L CNN
	1    2800 4600
	1    0    0    -1  
$EndComp
$Comp
L Device:R 330R?
U 1 1 6094F0F6
P 2250 6150
F 0 "330R?" V 2043 6150 50  0001 C CNN
F 1 "330R" V 2134 6150 50  0000 C CNN
F 2 "" V 2180 6150 50  0001 C CNN
F 3 "~" H 2250 6150 50  0001 C CNN
	1    2250 6150
	0    1    1    0   
$EndComp
Wire Wire Line
	1850 4500 2100 4500
Wire Wire Line
	2100 5100 1850 5100
Wire Wire Line
	1850 5100 1850 4500
Wire Wire Line
	1850 6150 2100 6150
Wire Wire Line
	2400 4500 2500 4500
Wire Wire Line
	2400 5100 2500 5100
Wire Wire Line
	2400 6150 2500 6150
$Comp
L Device:D D?
U 1 1 60994B0C
P 3650 5100
F 0 "D?" H 3650 5317 50  0001 C CNN
F 1 "D" H 3650 5226 50  0000 C CNN
F 2 "" H 3650 5100 50  0001 C CNN
F 3 "~" H 3650 5100 50  0001 C CNN
	1    3650 5100
	1    0    0    -1  
$EndComp
$Comp
L Device:D D?
U 1 1 609A6157
P 3650 6150
F 0 "D?" H 3650 6367 50  0001 C CNN
F 1 "D" H 3650 6276 50  0000 C CNN
F 2 "" H 3650 6150 50  0001 C CNN
F 3 "~" H 3650 6150 50  0001 C CNN
	1    3650 6150
	1    0    0    -1  
$EndComp
$Comp
L Connector:RJ45 Radio
U 1 1 6093A75A
P 10550 4100
F 0 "Radio" H 10220 4196 50  0000 R CNN
F 1 "RJ45" H 10220 4105 50  0000 R CNN
F 2 "" V 10550 4125 50  0001 C CNN
F 3 "~" V 10550 4125 50  0001 C CNN
	1    10550 4100
	-1   0    0    1   
$EndComp
$Comp
L Device:C C_in
U 1 1 60943DC5
P 8700 3600
F 0 "C_in" V 8472 3600 39  0000 C CNN
F 1 "10uf" V 8547 3600 39  0000 C CNN
F 2 "" H 8738 3450 50  0001 C CNN
F 3 "~" H 8700 3600 50  0001 C CNN
	1    8700 3600
	0    1    1    0   
$EndComp
$Comp
L Device:C C_out
U 1 1 60946D54
P 8700 4100
F 0 "C_out" V 8472 4100 39  0000 C CNN
F 1 "10uf" V 8547 4100 39  0000 C CNN
F 2 "" H 8738 3950 50  0001 C CNN
F 3 "~" H 8700 4100 50  0001 C CNN
	1    8700 4100
	0    1    1    0   
$EndComp
Text GLabel 9500 4000 0    39   Input ~ 0
PTT
Text GLabel 9500 4300 0    39   Output ~ 0
TXDF
Text GLabel 9550 4500 0    39   Input ~ 0
ON_RXDF
$Comp
L power:+8V #8V
U 1 1 60998F91
P 10100 3500
F 0 "#8V" H 10115 3741 39  0001 C CNN
F 1 "+8V" H 10115 3665 39  0000 C CNN
F 2 "" H 10100 3500 50  0001 C CNN
F 3 "" H 10100 3500 50  0001 C CNN
	1    10100 3500
	1    0    0    -1  
$EndComp
$Comp
L power:GND1 #GND1
U 1 1 609995F5
P 10100 4700
F 0 "#GND1" H 10100 4450 39  0001 C CNN
F 1 "GND1" H 10105 4535 39  0000 C CNN
F 2 "" H 10100 4700 50  0001 C CNN
F 3 "" H 10100 4700 50  0001 C CNN
	1    10100 4700
	1    0    0    -1  
$EndComp
Wire Wire Line
	10100 3500 10100 3800
Wire Wire Line
	10100 4400 10100 4700
$Comp
L Connector:AudioPlug3 MIC_on_PC
U 1 1 609AF815
P 7600 3700
F 0 "MIC_on_PC" H 7070 3654 39  0000 R CNN
F 1 "AudioPlug" H 7070 3745 39  0000 R CNN
F 2 "" H 7700 3650 50  0001 C CNN
F 3 "~" H 7700 3650 50  0001 C CNN
	1    7600 3700
	1    0    0    1   
$EndComp
$Comp
L Connector:AudioPlug3 Speaker_on_PC
U 1 1 609CDB57
P 7600 4300
F 0 "Speaker_on_PC" H 7070 4254 39  0000 R CNN
F 1 "AudioPlug" H 7070 4345 39  0000 R CNN
F 2 "" H 7700 4250 50  0001 C CNN
F 3 "~" H 7700 4250 50  0001 C CNN
	1    7600 4300
	1    0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x08 USB_to_TTL
U 1 1 609D87E9
P 1750 2150
F 0 "USB_to_TTL" H 1668 2667 50  0000 C CNN
F 1 "CP2102" H 1668 2576 50  0000 C CNN
F 2 "" H 1750 2150 50  0001 C CNN
F 3 "~" H 1750 2150 50  0001 C CNN
	1    1750 2150
	-1   0    0    -1  
$EndComp
Text GLabel 2100 2050 2    39   Output ~ 0
TXD
Text GLabel 2100 2150 2    39   Input ~ 0
RXD
Text GLabel 2100 2250 2    39   Output ~ 0
DTR
Text GLabel 2100 2350 2    39   Output ~ 0
RTS
$Comp
L power:GND2 #GND2
U 1 1 609ED425
P 2250 2650
F 0 "#GND2" H 2250 2400 39  0001 C CNN
F 1 "GND2" H 2255 2485 39  0000 C CNN
F 2 "" H 2250 2650 50  0001 C CNN
F 3 "" H 2250 2650 50  0001 C CNN
	1    2250 2650
	1    0    0    -1  
$EndComp
$Comp
L power:+3.3V #3.3V
U 1 1 609EE13A
P 2250 1700
F 0 "#3.3V" H 2250 1550 39  0001 C CNN
F 1 "+3.3V" H 2265 1865 39  0000 C CNN
F 2 "" H 2250 1700 50  0001 C CNN
F 3 "" H 2250 1700 50  0001 C CNN
	1    2250 1700
	1    0    0    -1  
$EndComp
Wire Wire Line
	2250 1700 2250 1850
Wire Wire Line
	2250 1850 1950 1850
Wire Wire Line
	1950 2550 2250 2550
Wire Wire Line
	2250 2550 2250 2650
Wire Wire Line
	2100 2350 1950 2350
Wire Wire Line
	1950 2250 2100 2250
Wire Wire Line
	2100 2150 1950 2150
Wire Wire Line
	1950 2050 2100 2050
Text GLabel 2350 4700 0    39   Output ~ 0
RTS
Wire Wire Line
	2350 4700 2500 4700
Text GLabel 2350 5300 0    39   Output ~ 0
DTR
Wire Wire Line
	2500 5300 2350 5300
Text GLabel 2350 6350 0    39   Output ~ 0
TXD
Text GLabel 2350 6800 0    39   Input ~ 0
RXD
Wire Wire Line
	2350 6350 2500 6350
Wire Wire Line
	2400 6700 2400 6800
Wire Wire Line
	2400 6800 2350 6800
Wire Wire Line
	2400 6700 2500 6700
$Comp
L power:GND2 #GND?
U 1 1 60A5B407
P 1850 7100
F 0 "#GND?" H 1850 6850 39  0001 C CNN
F 1 "GND2" H 1855 6935 39  0000 C CNN
F 2 "" H 1850 7100 50  0001 C CNN
F 3 "" H 1850 7100 50  0001 C CNN
	1    1850 7100
	1    0    0    -1  
$EndComp
Wire Wire Line
	2500 6900 1850 6900
Wire Wire Line
	1850 6900 1850 7100
$Comp
L power:GND1 #GND?
U 1 1 60A65AB4
P 3300 7100
F 0 "#GND?" H 3300 6850 39  0001 C CNN
F 1 "GND1" H 3305 6935 39  0000 C CNN
F 2 "" H 3300 7100 50  0001 C CNN
F 3 "" H 3300 7100 50  0001 C CNN
	1    3300 7100
	1    0    0    -1  
$EndComp
Text GLabel 4350 6150 2    39   Input ~ 0
ON_RXDF
Text GLabel 3950 4500 2    39   Input ~ 0
PTT
Text GLabel 4300 6700 2    39   Output ~ 0
TXDF
Wire Wire Line
	3100 4700 3300 4700
Wire Wire Line
	3300 4700 3300 5300
Wire Wire Line
	3100 6900 3300 6900
Connection ~ 3300 6900
Wire Wire Line
	3300 6900 3300 7100
Wire Wire Line
	3100 6350 3300 6350
Wire Wire Line
	3300 6350 3300 6900
Wire Wire Line
	3100 5300 3300 5300
Wire Wire Line
	3100 4500 3500 4500
Wire Wire Line
	3100 6700 3500 6700
Wire Wire Line
	4200 6150 4350 6150
Wire Wire Line
	3800 6700 4300 6700
Wire Wire Line
	2400 6700 1850 6700
Wire Wire Line
	1850 6700 1850 6600
Connection ~ 2400 6700
Text GLabel 9500 4200 0    39   Input ~ 0
VPP
Wire Wire Line
	3950 4500 3800 4500
$Comp
L power:+3.3V #3.3V
U 1 1 609B5B06
P 1850 4000
F 0 "#3.3V" H 1850 3850 39  0001 C CNN
F 1 "+3.3V" H 1865 4165 39  0000 C CNN
F 2 "" H 1850 4000 50  0001 C CNN
F 3 "" H 1850 4000 50  0001 C CNN
	1    1850 4000
	1    0    0    -1  
$EndComp
Wire Wire Line
	3800 5100 3850 5100
Wire Wire Line
	3500 6150 3100 6150
Wire Wire Line
	3100 5100 3500 5100
Connection ~ 3300 5300
Wire Wire Line
	1850 4000 1850 4500
Wire Wire Line
	10150 3900 9850 3900
Connection ~ 10100 4400
Wire Wire Line
	10100 4400 10150 4400
Wire Wire Line
	10100 3800 10150 3800
Wire Wire Line
	8850 3600 9850 3600
Wire Wire Line
	1850 6150 1850 6300
Wire Wire Line
	8200 4400 8400 4400
Wire Wire Line
	8200 3800 8400 3800
Wire Wire Line
	8400 3800 8400 4400
Connection ~ 8400 4400
Wire Wire Line
	8200 4200 8300 4200
Wire Wire Line
	8300 4200 8300 4100
Wire Wire Line
	8300 4100 8550 4100
Wire Wire Line
	9850 3900 9850 3600
Wire Wire Line
	8200 3600 8550 3600
Wire Wire Line
	3800 6150 3850 6150
Wire Wire Line
	3850 5100 3850 6150
Connection ~ 3850 6150
Wire Wire Line
	3850 6150 3900 6150
Wire Wire Line
	3300 5300 3300 6350
Connection ~ 3300 6350
Connection ~ 1850 4500
Wire Wire Line
	1850 5100 1850 6150
Connection ~ 1850 5100
Connection ~ 1850 6150
Wire Wire Line
	9550 4500 10150 4500
Wire Wire Line
	8400 4400 10100 4400
Wire Wire Line
	9500 4300 10150 4300
Wire Wire Line
	9500 4200 10150 4200
Wire Wire Line
	8850 4100 10150 4100
Wire Wire Line
	9500 4000 10150 4000
$EndSCHEMATC
