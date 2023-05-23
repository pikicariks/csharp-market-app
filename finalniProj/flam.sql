/*
Navicat MySQL Data Transfer

Source Server         : sffas
Source Server Version : 50541
Source Host           : localhost:3306
Source Database       : flam

Target Server Type    : MYSQL
Target Server Version : 50541
File Encoding         : 65001

Date: 2022-04-30 11:11:26
*/
CREATE DATABASE flamingo;
USE flamingo;

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `artikal`
-- ----------------------------
DROP TABLE IF EXISTS `artikal`;
CREATE TABLE `artikal` (
  `artikal_id` int(10) NOT NULL AUTO_INCREMENT,
  `naziv_artikla` varchar(50) NOT NULL DEFAULT '',
  `vrsta_artikla` varchar(50) DEFAULT NULL,
  `cijena` double(8,4) DEFAULT NULL,
  PRIMARY KEY (`artikal_id`,`naziv_artikla`),
  KEY `idx_naziv_artikla` (`naziv_artikla`),
  KEY `artikal_id` (`artikal_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of artikal
-- ----------------------------
INSERT INTO `artikal` VALUES ('1', 'Paracetamol', 'Lijekovi', '69.7000');
INSERT INTO `artikal` VALUES ('2', 'Razer miš', 'Kompjuterska oprema', '230.0000');
INSERT INTO `artikal` VALUES ('3', 'Polubijeli hljeb', 'Hrana', '93.6000');
INSERT INTO `artikal` VALUES ('4', 'Olimpijska šipka', 'Oprema za teretanu', '84.6500');
INSERT INTO `artikal` VALUES ('5', 'Tene', 'Obuća', '120.9000');

-- ----------------------------
-- Table structure for `kupac`
-- ----------------------------
DROP TABLE IF EXISTS `kupac`;
CREATE TABLE `kupac` (
  `kupac_id` int(10) NOT NULL AUTO_INCREMENT,
  `ime` varchar(20) NOT NULL,
  `prezime` varchar(20) NOT NULL,
  `grad` varchar(20) DEFAULT NULL,
  `adresa` varchar(40) DEFAULT NULL,
  `telefon` varchar(20) DEFAULT NULL,
  `user` varchar(40) DEFAULT NULL,
  `pass` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`kupac_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of kupac
-- ----------------------------
INSERT INTO `kupac` VALUES ('1', 'Admin', 'Admin', 'Admin', 'Admin ', '062062062', 'admin.a', '1admin.a');
INSERT INTO `kupac` VALUES ('2', 'Fake', 'Fakili', 'aa', 'a23', '', 'fake.f', '2fake.f');
INSERT INTO `kupac` VALUES ('3', 'Misko', 'Mukovic', 'Grbavica', 'Grbo 69', '0606060', 'misko.o', '3misko.m');
INSERT INTO `kupac` VALUES ('4', 'Ricko', 'Rickovic', 'Santijago', 'Safeta Zajke 50', '061123321', 'ricko.r', '4ricko.r');
INSERT INTO `kupac` VALUES ('5', 'Faruk', 'Farukovic', 'Hrid', 'Oteska 69', '61213312', 'faruk.f', '5faruk.f');
INSERT INTO `kupac` VALUES ('6', 'Emir', 'Duve', 'Sarajevo', 'abicka 43a', '05663464644', 'vedu', '6duve.d');

-- ----------------------------
-- Table structure for `narudzbenica`
-- ----------------------------
DROP TABLE IF EXISTS `narudzbenica`;
CREATE TABLE `narudzbenica` (
  `narudzbenica_id` int(10) NOT NULL AUTO_INCREMENT,
  `kupac_id` int(10) NOT NULL,
  `datum_narudzbe` date DEFAULT NULL,
  PRIMARY KEY (`narudzbenica_id`),
  KEY `narudzbenica_kupac` (`kupac_id`),
  CONSTRAINT `narudzbenica_kupac` FOREIGN KEY (`kupac_id`) REFERENCES `kupac` (`kupac_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of narudzbenica
-- ----------------------------
INSERT INTO `narudzbenica` VALUES ('1', '2', '2022-04-01');
INSERT INTO `narudzbenica` VALUES ('2', '2', '2022-04-20');
INSERT INTO `narudzbenica` VALUES ('3', '4', '2021-11-18');

-- ----------------------------
-- Table structure for `skladiste`
-- ----------------------------
DROP TABLE IF EXISTS `skladiste`;
CREATE TABLE `skladiste` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `artikal_id` int(10) NOT NULL,
  `naziv_artikla` varchar(50) DEFAULT NULL,
  `kolicina` int(10) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `skladiste_artikal` (`artikal_id`),
  KEY `Aritkal updejta skladiste` (`naziv_artikla`),
  CONSTRAINT `Aritkal updejta skladiste` FOREIGN KEY (`naziv_artikla`) REFERENCES `artikal` (`naziv_artikla`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `skladiste_artikal` FOREIGN KEY (`artikal_id`) REFERENCES `artikal` (`artikal_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='InnoDB free: 3072 kB; (`naziv_artikla`) REFER `projekat/arti';

-- ----------------------------
-- Records of skladiste
-- ----------------------------
INSERT INTO `skladiste` VALUES ('1', '1', 'Paracetamol', '58');
INSERT INTO `skladiste` VALUES ('2', '2', 'Razer miš', '18');
INSERT INTO `skladiste` VALUES ('3', '3', 'Polubijeli hljeb', '255');
INSERT INTO `skladiste` VALUES ('4', '4', 'Olimpijska šipka', '33');
INSERT INTO `skladiste` VALUES ('5', '5', 'Tene', '5');

-- ----------------------------
-- Table structure for `stavka_narudzbenice`
-- ----------------------------
DROP TABLE IF EXISTS `stavka_narudzbenice`;
CREATE TABLE `stavka_narudzbenice` (
  `stavka_id` int(10) NOT NULL AUTO_INCREMENT,
  `narudzbenica_id` int(10) NOT NULL,
  `artikal_id` int(10) NOT NULL,
  `kolicina` int(10) DEFAULT NULL,
  PRIMARY KEY (`stavka_id`),
  KEY `stavka_narudzbenica` (`narudzbenica_id`),
  KEY `stavka_artikal` (`artikal_id`),
  CONSTRAINT `stavka_narudzbenica` FOREIGN KEY (`narudzbenica_id`) REFERENCES `narudzbenica` (`narudzbenica_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `stavka_artikal` FOREIGN KEY (`artikal_id`) REFERENCES `artikal` (`artikal_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of stavka_narudzbenice
-- ----------------------------
