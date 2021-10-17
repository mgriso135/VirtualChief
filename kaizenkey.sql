-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: localhost    Database: kaizenkey
-- ------------------------------------------------------
-- Server version	8.0.11

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8mb4 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `anagraficaclienti`
--

DROP TABLE IF EXISTS `anagraficaclienti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `anagraficaclienti` (
  `codice` varchar(255) NOT NULL,
  `ragsociale` varchar(255) NOT NULL,
  `partitaiva` varchar(255) DEFAULT NULL,
  `codfiscale` varchar(255) DEFAULT NULL,
  `indirizzo` varchar(255) DEFAULT NULL,
  `citta` varchar(255) DEFAULT NULL,
  `provincia` varchar(255) DEFAULT NULL,
  `CAP` varchar(255) DEFAULT NULL,
  `stato` varchar(255) DEFAULT NULL,
  `telefono` varchar(45) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `kanbanManaged` bit(1) NOT NULL DEFAULT b'0',
  `customer` bit(1) NOT NULL DEFAULT b'0',
  `provider` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`codice`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `anagraficaclienti`
--

LOCK TABLES `anagraficaclienti` WRITE;
/*!40000 ALTER TABLE `anagraficaclienti` DISABLE KEYS */;
INSERT INTO `anagraficaclienti` VALUES ('Testcustomer','Test customer',NULL,NULL,'','','','','','',NULL,'\0','\0','\0');
/*!40000 ALTER TABLE `anagraficaclienti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commesse`
--

DROP TABLE IF EXISTS `commesse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `commesse` (
  `idcommesse` int(11) NOT NULL,
  `anno` year(4) NOT NULL,
  `cliente` varchar(255) NOT NULL,
  `dataInserimento` datetime NOT NULL,
  `note` text,
  `confirmed` bit(1) NOT NULL DEFAULT b'0',
  `confirmedBy` varchar(45) DEFAULT NULL,
  `confirmedDate` datetime DEFAULT NULL,
  `ExternalID` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`idcommesse`,`anno`),
  KEY `FK_cliente_idx` (`cliente`),
  CONSTRAINT `FK_cliente` FOREIGN KEY (`cliente`) REFERENCES `anagraficaclienti` (`codice`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commesse`
--

LOCK TABLES `commesse` WRITE;
/*!40000 ALTER TABLE `commesse` DISABLE KEYS */;
INSERT INTO `commesse` VALUES (0,2021,'Testcustomer','2021-04-11 12:04:15','efsd','','google-oauth2|110995565931815493426','2021-04-11 12:04:15','123');
/*!40000 ALTER TABLE `commesse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `configurazione`
--

DROP TABLE IF EXISTS `configurazione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `configurazione` (
  `Sezione` varchar(255) NOT NULL,
  `ID` int(11) NOT NULL,
  `parametro` varchar(255) NOT NULL,
  `valore` varchar(255) NOT NULL,
  PRIMARY KEY (`Sezione`,`ID`,`parametro`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `configurazione`
--

LOCK TABLES `configurazione` WRITE;
/*!40000 ALTER TABLE `configurazione` DISABLE KEYS */;
INSERT INTO `configurazione` VALUES ('Andon Completo',-1,'FormatoUsername','0'),('Andon Completo',-1,'ScrollType','1;50000;20000'),('Andon ViewFields',-1,'CommessaCodiceCliente','1'),('Andon ViewFields',-1,'CommessaRagioneSocialeCliente','0'),('Andon ViewFields',-1,'EarlyStart','3'),('Andon ViewFields',-1,'ProdottoNomeProdotto','2'),('Andon ViewFieldsTasks',-1,'TaskDescrizione','1'),('Andon ViewFieldsTasks',-1,'TaskNome','0'),('Main',-1,'ExpiryDate','31/12/2021'),('Main',-1,'TimeZone','W. Europe Standard Time'),('Main',0,'Logo','../../Data/KaizenKey/Logo/logo.png'),('Wizard',-1,'TipoPERT','Graph');
/*!40000 ALTER TABLE `configurazione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contatticlienti`
--

DROP TABLE IF EXISTS `contatticlienti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `contatticlienti` (
  `idContatto` int(11) NOT NULL,
  `cliente` varchar(255) NOT NULL,
  `firstname` varchar(255) DEFAULT NULL,
  `lastname` varchar(255) DEFAULT NULL,
  `ruolo` varchar(255) DEFAULT NULL,
  `user` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`idContatto`),
  KEY `FKcontatto_cliente_idx` (`cliente`),
  KEY `FKcontatticliente_user_idx` (`user`),
  CONSTRAINT `FKcontatticliente_user` FOREIGN KEY (`user`) REFERENCES `users` (`userid`),
  CONSTRAINT `FKcontatto_cliente` FOREIGN KEY (`cliente`) REFERENCES `anagraficaclienti` (`codice`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contatticlienti`
--

LOCK TABLES `contatticlienti` WRITE;
/*!40000 ALTER TABLE `contatticlienti` DISABLE KEYS */;
/*!40000 ALTER TABLE `contatticlienti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contatticlienti_email`
--

DROP TABLE IF EXISTS `contatticlienti_email`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `contatticlienti_email` (
  `idContatto` int(11) NOT NULL,
  `email` varchar(255) NOT NULL,
  `note` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`idContatto`,`email`),
  KEY `FK_contatto_email_idx` (`idContatto`),
  CONSTRAINT `FK_contatto_email` FOREIGN KEY (`idContatto`) REFERENCES `contatticlienti` (`idcontatto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contatticlienti_email`
--

LOCK TABLES `contatticlienti_email` WRITE;
/*!40000 ALTER TABLE `contatticlienti_email` DISABLE KEYS */;
/*!40000 ALTER TABLE `contatticlienti_email` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contatticlienti_phone`
--

DROP TABLE IF EXISTS `contatticlienti_phone`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `contatticlienti_phone` (
  `idContatto` int(11) NOT NULL,
  `phone` varchar(255) NOT NULL,
  `note` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`idContatto`,`phone`),
  KEY `contattiphone_FK_contatto_idx` (`idContatto`),
  CONSTRAINT `contattiphone_FK_contatto` FOREIGN KEY (`idContatto`) REFERENCES `contatticlienti` (`idcontatto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contatticlienti_phone`
--

LOCK TABLES `contatticlienti_phone` WRITE;
/*!40000 ALTER TABLE `contatticlienti_phone` DISABLE KEYS */;
/*!40000 ALTER TABLE `contatticlienti_phone` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `correctiveactions`
--

DROP TABLE IF EXISTS `correctiveactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `correctiveactions` (
  `ID` int(11) NOT NULL,
  `ImprovementActionID` int(11) NOT NULL,
  `ImprovementActionYear` year(4) NOT NULL,
  `Description` text,
  `LeadTimeExpected` double DEFAULT NULL COMMENT 'giorni di lavoro previsti',
  `EarlyStart` datetime DEFAULT NULL,
  `LateStart` datetime DEFAULT NULL,
  `EarlyFinish` datetime DEFAULT NULL,
  `LateFinish` datetime DEFAULT NULL,
  `Status` char(1) DEFAULT NULL COMMENT '{O = Aperta, C = Chiusa}',
  `EndDateReal` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`,`ImprovementActionID`,`ImprovementActionYear`),
  KEY `ImprovementActions_AC_FK_idx` (`ImprovementActionID`,`ImprovementActionYear`),
  CONSTRAINT `ImprovementActions_AC_FK` FOREIGN KEY (`ImprovementActionID`, `ImprovementActionYear`) REFERENCES `improvementactions` (`id`, `year`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `correctiveactions`
--

LOCK TABLES `correctiveactions` WRITE;
/*!40000 ALTER TABLE `correctiveactions` DISABLE KEYS */;
/*!40000 ALTER TABLE `correctiveactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `correctiveactions_tasks`
--

DROP TABLE IF EXISTS `correctiveactions_tasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `correctiveactions_tasks` (
  `ImprovementActionID` int(11) NOT NULL,
  `ImprovementActionYear` year(4) NOT NULL,
  `CorrectiveActionID` int(11) NOT NULL,
  `TaskID` int(11) NOT NULL,
  `Description` text,
  `User` varchar(255) NOT NULL,
  `Date` datetime NOT NULL,
  PRIMARY KEY (`ImprovementActionID`,`ImprovementActionYear`,`CorrectiveActionID`,`TaskID`),
  KEY `CA_Tasks_user_FK` (`User`),
  CONSTRAINT `CA_Tasks_FK` FOREIGN KEY (`ImprovementActionID`, `ImprovementActionYear`, `CorrectiveActionID`) REFERENCES `correctiveactions` (`improvementactionid`, `improvementactionyear`, `id`),
  CONSTRAINT `CA_Tasks_user_FK` FOREIGN KEY (`User`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `correctiveactions_tasks`
--

LOCK TABLES `correctiveactions_tasks` WRITE;
/*!40000 ALTER TABLE `correctiveactions_tasks` DISABLE KEYS */;
/*!40000 ALTER TABLE `correctiveactions_tasks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `correctiveactions_team`
--

DROP TABLE IF EXISTS `correctiveactions_team`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `correctiveactions_team` (
  `CorrectiveActionID` int(11) NOT NULL,
  `ImprovementActionID` int(11) NOT NULL,
  `ImprovementActionYear` year(4) NOT NULL,
  `User` varchar(255) NOT NULL,
  `Role` char(1) NOT NULL,
  PRIMARY KEY (`CorrectiveActionID`,`ImprovementActionID`,`ImprovementActionYear`,`User`),
  KEY `CA_Team_user_FK` (`User`),
  CONSTRAINT `CA_Team_user_FK` FOREIGN KEY (`User`) REFERENCES `users` (`userid`),
  CONSTRAINT `CorrectiveAction_Team_FK` FOREIGN KEY (`CorrectiveActionID`, `ImprovementActionID`, `ImprovementActionYear`) REFERENCES `correctiveactions` (`id`, `improvementactionid`, `improvementactionyear`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `correctiveactions_team`
--

LOCK TABLES `correctiveactions_team` WRITE;
/*!40000 ALTER TABLE `correctiveactions_team` DISABLE KEYS */;
/*!40000 ALTER TABLE `correctiveactions_team` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventitasksproduzione`
--

DROP TABLE IF EXISTS `eventitasksproduzione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventitasksproduzione` (
  `task` int(11) NOT NULL,
  `cadenza` int(11) NOT NULL,
  `evento` varchar(1) NOT NULL,
  `ora` datetime NOT NULL,
  KEY `task` (`task`),
  CONSTRAINT `eventitasksproduzione_ibfk_1` FOREIGN KEY (`task`) REFERENCES `tasksproduzione` (`taskid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventitasksproduzione`
--

LOCK TABLES `eventitasksproduzione` WRITE;
/*!40000 ALTER TABLE `eventitasksproduzione` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventitasksproduzione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventoarticoloconfig`
--

DROP TABLE IF EXISTS `eventoarticoloconfig`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventoarticoloconfig` (
  `TipoEvento` varchar(255) NOT NULL,
  `ArticoloID` int(11) NOT NULL,
  `ArticoloAnno` year(4) NOT NULL,
  `RitardoMinimoDaSegnalare` time NOT NULL,
  PRIMARY KEY (`TipoEvento`,`ArticoloID`,`ArticoloAnno`),
  KEY `eventoarticoloconfig_FK1` (`ArticoloID`,`ArticoloAnno`),
  CONSTRAINT `eventoarticoloconfig_FK1` FOREIGN KEY (`ArticoloID`, `ArticoloAnno`) REFERENCES `productionplan` (`id`, `anno`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventoarticoloconfig`
--

LOCK TABLES `eventoarticoloconfig` WRITE;
/*!40000 ALTER TABLE `eventoarticoloconfig` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventoarticoloconfig` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventoarticologruppi`
--

DROP TABLE IF EXISTS `eventoarticologruppi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventoarticologruppi` (
  `TipoEvento` varchar(255) NOT NULL,
  `ArticoloID` int(11) NOT NULL,
  `ArticoloAnno` year(4) NOT NULL,
  `idGruppo` int(11) NOT NULL,
  PRIMARY KEY (`TipoEvento`,`idGruppo`,`ArticoloAnno`,`ArticoloID`),
  KEY `eventoarticologruppi_FK1` (`ArticoloID`,`ArticoloAnno`),
  KEY `eventoarticologruppi_FK2` (`idGruppo`),
  CONSTRAINT `eventoarticologruppi_FK1` FOREIGN KEY (`ArticoloID`, `ArticoloAnno`) REFERENCES `productionplan` (`id`, `anno`),
  CONSTRAINT `eventoarticologruppi_FK2` FOREIGN KEY (`idGruppo`) REFERENCES `groupss` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventoarticologruppi`
--

LOCK TABLES `eventoarticologruppi` WRITE;
/*!40000 ALTER TABLE `eventoarticologruppi` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventoarticologruppi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventoarticoloutenti`
--

DROP TABLE IF EXISTS `eventoarticoloutenti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventoarticoloutenti` (
  `TipoEvento` varchar(255) NOT NULL,
  `ArticoloID` int(11) NOT NULL,
  `ArticoloAnno` year(4) NOT NULL,
  `userID` varchar(255) NOT NULL,
  PRIMARY KEY (`TipoEvento`,`userID`,`ArticoloAnno`,`ArticoloID`),
  KEY `eventoarticoloutenti_FK1` (`ArticoloID`,`ArticoloAnno`),
  KEY `eventoarticoloutenti_FK2` (`userID`),
  CONSTRAINT `eventoarticoloutenti_FK1` FOREIGN KEY (`ArticoloID`, `ArticoloAnno`) REFERENCES `productionplan` (`id`, `anno`),
  CONSTRAINT `eventoarticoloutenti_FK2` FOREIGN KEY (`userID`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventoarticoloutenti`
--

LOCK TABLES `eventoarticoloutenti` WRITE;
/*!40000 ALTER TABLE `eventoarticoloutenti` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventoarticoloutenti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventocommessaconfig`
--

DROP TABLE IF EXISTS `eventocommessaconfig`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventocommessaconfig` (
  `TipoEvento` varchar(255) NOT NULL,
  `CommessaID` int(11) NOT NULL,
  `CommessaAnno` year(4) NOT NULL,
  `RitardoMinimoDaSegnalare` time NOT NULL,
  PRIMARY KEY (`TipoEvento`,`CommessaAnno`,`CommessaID`),
  KEY `eventocommessaconfig_FK1` (`CommessaID`,`CommessaAnno`),
  CONSTRAINT `eventocommessaconfig_FK1` FOREIGN KEY (`CommessaID`, `CommessaAnno`) REFERENCES `commesse` (`idcommesse`, `anno`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventocommessaconfig`
--

LOCK TABLES `eventocommessaconfig` WRITE;
/*!40000 ALTER TABLE `eventocommessaconfig` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventocommessaconfig` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventocommessagruppi`
--

DROP TABLE IF EXISTS `eventocommessagruppi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventocommessagruppi` (
  `TipoEvento` varchar(255) NOT NULL,
  `CommessaID` int(11) NOT NULL,
  `CommessaAnno` year(4) NOT NULL,
  `idGruppo` int(11) NOT NULL,
  PRIMARY KEY (`TipoEvento`,`CommessaID`,`CommessaAnno`,`idGruppo`),
  KEY `eventocommessagruppi_FK1` (`CommessaID`,`CommessaAnno`),
  KEY `eventocommessagruppi_FK2` (`idGruppo`),
  CONSTRAINT `eventocommessagruppi_FK1` FOREIGN KEY (`CommessaID`, `CommessaAnno`) REFERENCES `commesse` (`idcommesse`, `anno`),
  CONSTRAINT `eventocommessagruppi_FK2` FOREIGN KEY (`idGruppo`) REFERENCES `groupss` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventocommessagruppi`
--

LOCK TABLES `eventocommessagruppi` WRITE;
/*!40000 ALTER TABLE `eventocommessagruppi` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventocommessagruppi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventocommessautenti`
--

DROP TABLE IF EXISTS `eventocommessautenti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventocommessautenti` (
  `TipoEvento` varchar(255) NOT NULL,
  `CommessaID` int(11) NOT NULL,
  `CommessaAnno` year(4) NOT NULL,
  `userID` varchar(255) NOT NULL,
  PRIMARY KEY (`TipoEvento`,`CommessaID`,`CommessaAnno`,`userID`),
  KEY `eventocommessautenti_FK1` (`CommessaID`,`CommessaAnno`),
  KEY `eventocommessautenti_FK2` (`userID`),
  CONSTRAINT `eventocommessautenti_FK1` FOREIGN KEY (`CommessaID`, `CommessaAnno`) REFERENCES `commesse` (`idcommesse`, `anno`),
  CONSTRAINT `eventocommessautenti_FK2` FOREIGN KEY (`userID`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventocommessautenti`
--

LOCK TABLES `eventocommessautenti` WRITE;
/*!40000 ALTER TABLE `eventocommessautenti` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventocommessautenti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventorepartoconfig`
--

DROP TABLE IF EXISTS `eventorepartoconfig`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventorepartoconfig` (
  `TipoEvento` varchar(255) NOT NULL,
  `Reparto` int(11) NOT NULL,
  `RitardoMinimoDaSegnalare` time NOT NULL,
  PRIMARY KEY (`TipoEvento`,`Reparto`),
  KEY `eventorepartoconfig_FK1` (`Reparto`),
  CONSTRAINT `eventorepartoconfig_FK1` FOREIGN KEY (`Reparto`) REFERENCES `reparti` (`idreparto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventorepartoconfig`
--

LOCK TABLES `eventorepartoconfig` WRITE;
/*!40000 ALTER TABLE `eventorepartoconfig` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventorepartoconfig` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventorepartogruppi`
--

DROP TABLE IF EXISTS `eventorepartogruppi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventorepartogruppi` (
  `TipoEvento` varchar(255) NOT NULL,
  `idReparto` int(11) NOT NULL,
  `idGruppo` int(11) NOT NULL,
  PRIMARY KEY (`TipoEvento`,`idReparto`,`idGruppo`),
  KEY `evRepGruppi_FK1` (`idReparto`),
  KEY `evRepGruppi_FK2` (`idGruppo`),
  CONSTRAINT `evRepGruppi_FK1` FOREIGN KEY (`idReparto`) REFERENCES `reparti` (`idreparto`),
  CONSTRAINT `evRepGruppi_FK2` FOREIGN KEY (`idGruppo`) REFERENCES `groupss` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventorepartogruppi`
--

LOCK TABLES `eventorepartogruppi` WRITE;
/*!40000 ALTER TABLE `eventorepartogruppi` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventorepartogruppi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventorepartoutenti`
--

DROP TABLE IF EXISTS `eventorepartoutenti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `eventorepartoutenti` (
  `TipoEvento` varchar(255) NOT NULL,
  `repartoID` int(11) NOT NULL,
  `userID` varchar(255) NOT NULL,
  PRIMARY KEY (`TipoEvento`,`repartoID`,`userID`),
  KEY `evrepartoutenti_FK1` (`repartoID`),
  KEY `evrepartoutenti_FK2` (`userID`),
  CONSTRAINT `evrepartoutenti_FK1` FOREIGN KEY (`repartoID`) REFERENCES `reparti` (`idreparto`),
  CONSTRAINT `evrepartoutenti_FK2` FOREIGN KEY (`userID`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventorepartoutenti`
--

LOCK TABLES `eventorepartoutenti` WRITE;
/*!40000 ALTER TABLE `eventorepartoutenti` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventorepartoutenti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `freemeasurements`
--

DROP TABLE IF EXISTS `freemeasurements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `freemeasurements` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `creationdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `createdby` varchar(255) DEFAULT NULL,
  `plannedstartdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `plannedenddate` datetime DEFAULT NULL,
  `departmentid` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `description` text,
  `processid` int(11) NOT NULL,
  `processrev` int(11) NOT NULL,
  `variantid` int(11) NOT NULL,
  `status` char(1) NOT NULL DEFAULT 'N' COMMENT 'N = Not started\nI = Running\nP = Paused\nF = Finished',
  `serialnumber` varchar(45) DEFAULT NULL,
  `quantity` double NOT NULL DEFAULT '1',
  `measurementUnit` int(11) NOT NULL DEFAULT '0',
  `realenddate` datetime DEFAULT NULL,
  `realworkingtime_hours` double DEFAULT NULL,
  `realleadtime_hours` double DEFAULT NULL,
  `AllowCustomTasks` bit(1) NOT NULL DEFAULT b'1',
  `ExecuteFinishedTasks` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`),
  KEY `freemeasurements_FK2_idx` (`processid`,`processrev`,`variantid`),
  KEY `freemeasurements_FK3_idx` (`measurementUnit`),
  KEY `freemeasurements_FK1_idx` (`departmentid`),
  CONSTRAINT `freemeasurements_FK1` FOREIGN KEY (`departmentid`) REFERENCES `reparti` (`idreparto`),
  CONSTRAINT `freemeasurements_FK2` FOREIGN KEY (`processid`, `processrev`, `variantid`) REFERENCES `variantiprocessi` (`processo`, `revproc`, `variante`),
  CONSTRAINT `freemeasurements_FK3` FOREIGN KEY (`measurementUnit`) REFERENCES `measurementunits` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `freemeasurements`
--

LOCK TABLES `freemeasurements` WRITE;
/*!40000 ALTER TABLE `freemeasurements` DISABLE KEYS */;
INSERT INTO `freemeasurements` VALUES (31,'2021-06-27 21:10:48','13','2021-06-28 00:00:00','2021-06-30 00:00:00',0,'Test','Test',16,0,5,'N','sdsa',2,0,NULL,NULL,NULL,'','');
/*!40000 ALTER TABLE `freemeasurements` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `freemeasurements_errors_in_tasks`
--

DROP TABLE IF EXISTS `freemeasurements_errors_in_tasks`;
/*!50001 DROP VIEW IF EXISTS `freemeasurements_errors_in_tasks`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `freemeasurements_errors_in_tasks` AS SELECT 
 1 AS `MeasurementId`,
 1 AS `TaskId`,
 1 AS `OrigTaskId`,
 1 AS `OrigTaskRev`,
 1 AS `VariantId`,
 1 AS `NoProductiveTaskId`,
 1 AS `name`,
 1 AS `description`,
 1 AS `sequence`,
 1 AS `workstationid`,
 1 AS `quantity_planned`,
 1 AS `quantity_produced`,
 1 AS `status`,
 1 AS `task_startdatereal`,
 1 AS `task_enddatereal`,
 1 AS `realleadtime_hours`,
 1 AS `realworkingtime_hours`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `freemeasurements_events_full_view`
--

DROP TABLE IF EXISTS `freemeasurements_events_full_view`;
/*!50001 DROP VIEW IF EXISTS `freemeasurements_events_full_view`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `freemeasurements_events_full_view` AS SELECT 
 1 AS `id`,
 1 AS `user`,
 1 AS `eventtype`,
 1 AS `eventdate`,
 1 AS `notes`,
 1 AS `MeasurementId`,
 1 AS `Taskid`,
 1 AS `OrigTaskId`,
 1 AS `OrigTaskRev`,
 1 AS `VariantId`,
 1 AS `NoProductiveTaskId`,
 1 AS `name`,
 1 AS `description`,
 1 AS `sequence`,
 1 AS `workstationid`,
 1 AS `quantity_planned`,
 1 AS `quantity_produced`,
 1 AS `TaskStatus`,
 1 AS `task_startdatereal`,
 1 AS `task_enddatereal`,
 1 AS `realleadtime_hours`,
 1 AS `realworkingtime_hours`,
 1 AS `FreeMeasurement_CreationDate`,
 1 AS `FreeMeasurement_CreatedBy`,
 1 AS `plannedstartdate`,
 1 AS `plannedenddate`,
 1 AS `MeasurementName`,
 1 AS `MeasurementDescription`,
 1 AS `FreeMeasurementsStatus`,
 1 AS `serialnumber`,
 1 AS `WorkstationName`,
 1 AS `DepartmentId`,
 1 AS `DepartmentName`,
 1 AS `ProcessName`,
 1 AS `ProcessDescription`,
 1 AS `VariantName`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `freemeasurements_operators_tasks_running`
--

DROP TABLE IF EXISTS `freemeasurements_operators_tasks_running`;
/*!50001 DROP VIEW IF EXISTS `freemeasurements_operators_tasks_running`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `freemeasurements_operators_tasks_running` AS SELECT 
 1 AS `user`,
 1 AS `TaskName`,
 1 AS `MeasurementName`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `freemeasurements_tasks`
--

DROP TABLE IF EXISTS `freemeasurements_tasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `freemeasurements_tasks` (
  `MeasurementId` int(11) NOT NULL,
  `TaskId` int(11) NOT NULL,
  `OrigTaskId` int(11) DEFAULT NULL,
  `OrigTaskRev` int(11) DEFAULT NULL,
  `VariantId` int(11) DEFAULT NULL,
  `NoProductiveTaskId` int(11) DEFAULT NULL,
  `name` varchar(255) NOT NULL,
  `description` text,
  `sequence` int(11) NOT NULL DEFAULT '1',
  `workstationid` int(11) DEFAULT NULL,
  `quantity_planned` double NOT NULL DEFAULT '1',
  `quantity_produced` double DEFAULT NULL,
  `status` char(1) NOT NULL DEFAULT 'N' COMMENT 'I = Running\nN = Not started\nP = Paused\nF = Finished',
  `task_startdatereal` datetime DEFAULT NULL,
  `task_enddatereal` datetime DEFAULT NULL,
  `realleadtime_hours` double DEFAULT NULL,
  `realworkingtime_hours` double DEFAULT NULL,
  PRIMARY KEY (`MeasurementId`,`TaskId`),
  KEY `freemeasurement_tasks_FK2_idx` (`OrigTaskId`,`OrigTaskRev`,`VariantId`),
  KEY `freemeasurement_tasks_FK2_idx1` (`NoProductiveTaskId`),
  KEY `freemeasurement_tasks_FK4_idx` (`workstationid`),
  CONSTRAINT `freemeasurement_tasks_FK1` FOREIGN KEY (`MeasurementId`) REFERENCES `freemeasurements` (`id`),
  CONSTRAINT `freemeasurement_tasks_FK2` FOREIGN KEY (`OrigTaskId`, `OrigTaskRev`, `VariantId`) REFERENCES `processipadrifigli` (`task`, `revtask`, `variante`),
  CONSTRAINT `freemeasurement_tasks_FK3` FOREIGN KEY (`NoProductiveTaskId`) REFERENCES `noproductivetasks` (`id`),
  CONSTRAINT `freemeasurement_tasks_FK4` FOREIGN KEY (`workstationid`) REFERENCES `postazioni` (`idpostazioni`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `freemeasurements_tasks`
--

LOCK TABLES `freemeasurements_tasks` WRITE;
/*!40000 ALTER TABLE `freemeasurements_tasks` DISABLE KEYS */;
INSERT INTO `freemeasurements_tasks` VALUES (31,0,6,0,5,NULL,'Prelievo accessori','Prelievo accessori (tutti i componeti necessari per l&#39;assemblaggio finale della porta)',1,22,2,NULL,'N',NULL,NULL,NULL,NULL),(31,1,7,0,5,NULL,'Prelievo profili','Prelievo profili in PVC e Alluminio per il taglio',2,22,2,NULL,'N',NULL,NULL,NULL,NULL),(31,2,8,0,5,NULL,'Taglio profili a misura','Taglio profili a misura PVC e Alluminio',3,21,2,NULL,'N',NULL,NULL,NULL,NULL),(31,3,9,0,5,NULL,'Preparazione per imballaggio','Preparazione per imballaggio',4,21,2,NULL,'N',NULL,NULL,NULL,NULL),(31,4,14,0,5,NULL,'Inserimento rinforzo su profili e saldatura','Inserimento rinforzo su profili e saldatura profili',5,21,2,NULL,'N',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `freemeasurements_tasks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `freemeasurements_tasks_created_by_operators`
--

DROP TABLE IF EXISTS `freemeasurements_tasks_created_by_operators`;
/*!50001 DROP VIEW IF EXISTS `freemeasurements_tasks_created_by_operators`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `freemeasurements_tasks_created_by_operators` AS SELECT 
 1 AS `id`,
 1 AS `creationdate`,
 1 AS `createdby`,
 1 AS `plannedstartdate`,
 1 AS `plannedenddate`,
 1 AS `departmentid`,
 1 AS `MeasurementName`,
 1 AS `MeasurementDescription`,
 1 AS `ProcessId`,
 1 AS `processrev`,
 1 AS `variantid`,
 1 AS `status`,
 1 AS `serialnumber`,
 1 AS `quantity`,
 1 AS `measurementUnit`,
 1 AS `type`,
 1 AS `taskid`,
 1 AS `origtaskid`,
 1 AS `origtaskrev`,
 1 AS `noproductivetaskid`,
 1 AS `TaskName`,
 1 AS `TaskDescription`,
 1 AS `sequence`,
 1 AS `workstationid`,
 1 AS `name`,
 1 AS `quantity_planned`,
 1 AS `TaskStatus`,
 1 AS `ProcessName`,
 1 AS `ProductName`,
 1 AS `task_startdatereal`,
 1 AS `task_enddatereal`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `freemeasurements_tasks_events`
--

DROP TABLE IF EXISTS `freemeasurements_tasks_events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `freemeasurements_tasks_events` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `freemeasurementid` int(11) NOT NULL,
  `taskid` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  `eventtype` char(1) NOT NULL,
  `eventdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `notes` text,
  PRIMARY KEY (`id`),
  KEY `freemeasurements_tasks_events_FK1_idx` (`freemeasurementid`,`taskid`),
  CONSTRAINT `freemeasurements_tasks_events_FK1` FOREIGN KEY (`freemeasurementid`, `taskid`) REFERENCES `freemeasurements_tasks` (`measurementid`, `taskid`)
) ENGINE=InnoDB AUTO_INCREMENT=796 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `freemeasurements_tasks_events`
--

LOCK TABLES `freemeasurements_tasks_events` WRITE;
/*!40000 ALTER TABLE `freemeasurements_tasks_events` DISABLE KEYS */;
/*!40000 ALTER TABLE `freemeasurements_tasks_events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `freemeasurements_tasks_events_timespans`
--

DROP TABLE IF EXISTS `freemeasurements_tasks_events_timespans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `freemeasurements_tasks_events_timespans` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `measurementid` int(11) NOT NULL,
  `taskid` int(11) NOT NULL,
  `inputpoint` varchar(255) NOT NULL COMMENT 'It is the user',
  `starteventid` int(11) NOT NULL,
  `starteventtype` char(1) NOT NULL,
  `starteventdate` datetime NOT NULL,
  `starteventnotes` text,
  `endeventid` int(11) NOT NULL,
  `endeventtype` char(1) NOT NULL,
  `endeventdate` datetime NOT NULL,
  `endeventnotes` text,
  PRIMARY KEY (`id`),
  KEY `starteevent_FK1_idx` (`starteventid`),
  KEY `endevent_FK1_idx` (`endeventid`),
  KEY `FMEventsTimespans_FK1_idx` (`measurementid`,`taskid`),
  CONSTRAINT `FMEventsTimespans_FK1` FOREIGN KEY (`measurementid`, `taskid`) REFERENCES `freemeasurements_tasks` (`measurementid`, `taskid`),
  CONSTRAINT `endevent_FK1` FOREIGN KEY (`endeventid`) REFERENCES `freemeasurements_tasks_events` (`id`),
  CONSTRAINT `starteevent_FK1` FOREIGN KEY (`starteventid`) REFERENCES `freemeasurements_tasks_events` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=283 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `freemeasurements_tasks_events_timespans`
--

LOCK TABLES `freemeasurements_tasks_events_timespans` WRITE;
/*!40000 ALTER TABLE `freemeasurements_tasks_events_timespans` DISABLE KEYS */;
/*!40000 ALTER TABLE `freemeasurements_tasks_events_timespans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `freemeasurements_timespans_full_view`
--

DROP TABLE IF EXISTS `freemeasurements_timespans_full_view`;
/*!50001 DROP VIEW IF EXISTS `freemeasurements_timespans_full_view`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `freemeasurements_timespans_full_view` AS SELECT 
 1 AS `MeasurementId`,
 1 AS `MeasurementName`,
 1 AS `Taskid`,
 1 AS `OrigTaskId`,
 1 AS `OrigTaskRev`,
 1 AS `VariantId`,
 1 AS `NoProductiveTaskId`,
 1 AS `TaskName`,
 1 AS `description`,
 1 AS `sequence`,
 1 AS `workstationid`,
 1 AS `WorkstationName`,
 1 AS `quantity_planned`,
 1 AS `quantity_produced`,
 1 AS `status`,
 1 AS `task_startdatereal`,
 1 AS `task_enddatereal`,
 1 AS `realleadtime_hours`,
 1 AS `realworkingtime_hours`,
 1 AS `id`,
 1 AS `inputpoint`,
 1 AS `starteventid`,
 1 AS `starteventtype`,
 1 AS `starteventdate`,
 1 AS `starteventnotes`,
 1 AS `endeventid`,
 1 AS `endeventtype`,
 1 AS `endeventdate`,
 1 AS `endeventnotes`,
 1 AS `Timespan_duration`,
 1 AS `DepartmentId`,
 1 AS `DepartmentName`,
 1 AS `ProcessName`,
 1 AS `ProcessDescription`,
 1 AS `VariantName`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `freemeasurements_timespans_full_view_no_productive`
--

DROP TABLE IF EXISTS `freemeasurements_timespans_full_view_no_productive`;
/*!50001 DROP VIEW IF EXISTS `freemeasurements_timespans_full_view_no_productive`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `freemeasurements_timespans_full_view_no_productive` AS SELECT 
 1 AS `MeasurementId`,
 1 AS `MeasurementName`,
 1 AS `Taskid`,
 1 AS `OrigTaskId`,
 1 AS `OrigTaskRev`,
 1 AS `VariantId`,
 1 AS `NoProductiveTaskId`,
 1 AS `TaskName`,
 1 AS `description`,
 1 AS `sequence`,
 1 AS `workstationid`,
 1 AS `WorkstationName`,
 1 AS `quantity_planned`,
 1 AS `quantity_produced`,
 1 AS `status`,
 1 AS `task_startdatereal`,
 1 AS `task_enddatereal`,
 1 AS `realleadtime_hours`,
 1 AS `realworkingtime_hours`,
 1 AS `id`,
 1 AS `inputpoint`,
 1 AS `starteventid`,
 1 AS `starteventtype`,
 1 AS `starteventdate`,
 1 AS `starteventnotes`,
 1 AS `endeventid`,
 1 AS `endeventtype`,
 1 AS `endeventdate`,
 1 AS `endeventnotes`,
 1 AS `Timespan_duration`,
 1 AS `DepartmentId`,
 1 AS `DepartmentName`,
 1 AS `ProcessName`,
 1 AS `ProcessDescription`,
 1 AS `VariantName`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `freemeasurements_timespans_full_view_productive`
--

DROP TABLE IF EXISTS `freemeasurements_timespans_full_view_productive`;
/*!50001 DROP VIEW IF EXISTS `freemeasurements_timespans_full_view_productive`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `freemeasurements_timespans_full_view_productive` AS SELECT 
 1 AS `MeasurementId`,
 1 AS `MeasurementName`,
 1 AS `Taskid`,
 1 AS `OrigTaskId`,
 1 AS `OrigTaskRev`,
 1 AS `VariantId`,
 1 AS `NoProductiveTaskId`,
 1 AS `TaskName`,
 1 AS `description`,
 1 AS `sequence`,
 1 AS `workstationid`,
 1 AS `WorkstationName`,
 1 AS `quantity_planned`,
 1 AS `quantity_produced`,
 1 AS `status`,
 1 AS `task_startdatereal`,
 1 AS `task_enddatereal`,
 1 AS `realleadtime_hours`,
 1 AS `realworkingtime_hours`,
 1 AS `id`,
 1 AS `inputpoint`,
 1 AS `starteventid`,
 1 AS `starteventtype`,
 1 AS `starteventdate`,
 1 AS `starteventnotes`,
 1 AS `endeventid`,
 1 AS `endeventtype`,
 1 AS `endeventdate`,
 1 AS `endeventnotes`,
 1 AS `Timespan_duration`,
 1 AS `DepartmentId`,
 1 AS `DepartmentName`,
 1 AS `ProcessName`,
 1 AS `ProcessDescription`,
 1 AS `VariantName`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `groupss`
--

DROP TABLE IF EXISTS `groupss`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `groupss` (
  `id` int(11) NOT NULL DEFAULT '0',
  `nomeGruppo` varchar(255) NOT NULL,
  `descrizione` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groupss`
--

LOCK TABLES `groupss` WRITE;
/*!40000 ALTER TABLE `groupss` DISABLE KEYS */;
/*!40000 ALTER TABLE `groupss` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groupusers`
--

DROP TABLE IF EXISTS `groupusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `groupusers` (
  `groupID` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  PRIMARY KEY (`groupID`,`user`),
  KEY `user` (`user`),
  CONSTRAINT `groupusers_ibfk_1` FOREIGN KEY (`groupID`) REFERENCES `groupss` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `groupusers_ibfk_2` FOREIGN KEY (`user`) REFERENCES `users` (`userid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groupusers`
--

LOCK TABLES `groupusers` WRITE;
/*!40000 ALTER TABLE `groupusers` DISABLE KEYS */;
/*!40000 ALTER TABLE `groupusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gruppipermessi`
--

DROP TABLE IF EXISTS `gruppipermessi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `gruppipermessi` (
  `idgroup` int(11) NOT NULL,
  `idpermesso` int(11) NOT NULL,
  `r` bit(1) DEFAULT NULL,
  `w` bit(1) DEFAULT NULL,
  `x` bit(1) DEFAULT NULL,
  PRIMARY KEY (`idgroup`,`idpermesso`),
  KEY `gruppipermessi_FK1` (`idgroup`),
  KEY `gruppipermessi_FK2` (`idpermesso`),
  CONSTRAINT `gruppipermessi_FK1` FOREIGN KEY (`idgroup`) REFERENCES `groupss` (`id`),
  CONSTRAINT `gruppipermessi_FK2` FOREIGN KEY (`idpermesso`) REFERENCES `permessi` (`idpermesso`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gruppipermessi`
--

LOCK TABLES `gruppipermessi` WRITE;
/*!40000 ALTER TABLE `gruppipermessi` DISABLE KEYS */;
/*!40000 ALTER TABLE `gruppipermessi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `homeboxesregistro`
--

DROP TABLE IF EXISTS `homeboxesregistro`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `homeboxesregistro` (
  `idHomeBox` int(11) NOT NULL,
  `nome` varchar(255) NOT NULL,
  `descrizione` text,
  `path` varchar(255) NOT NULL,
  PRIMARY KEY (`idHomeBox`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `homeboxesregistro`
--

LOCK TABLES `homeboxesregistro` WRITE;
/*!40000 ALTER TABLE `homeboxesregistro` DISABLE KEYS */;
INSERT INTO `homeboxesregistro` VALUES (0,'Prodotti programmati','Mostra i prossimi prodotti programmati','~/Produzione/HomeBoxNextProgrammedProducts.ascx');
/*!40000 ALTER TABLE `homeboxesregistro` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `homeboxesuser`
--

DROP TABLE IF EXISTS `homeboxesuser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `homeboxesuser` (
  `idHomeBox` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  `ordine` int(11) NOT NULL,
  PRIMARY KEY (`idHomeBox`,`user`),
  KEY `FK_boxes_user_idx` (`user`),
  KEY `FK_boxes_box_idx` (`idHomeBox`),
  CONSTRAINT `FK_boxes_box` FOREIGN KEY (`idHomeBox`) REFERENCES `homeboxesregistro` (`idhomebox`),
  CONSTRAINT `FK_boxes_user` FOREIGN KEY (`user`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `homeboxesuser`
--

LOCK TABLES `homeboxesuser` WRITE;
/*!40000 ALTER TABLE `homeboxesuser` DISABLE KEYS */;
/*!40000 ALTER TABLE `homeboxesuser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `improvementactions`
--

DROP TABLE IF EXISTS `improvementactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `improvementactions` (
  `ID` int(11) NOT NULL,
  `Year` year(4) NOT NULL,
  `CreatedBy` varchar(255) NOT NULL,
  `OpeningDate` datetime NOT NULL,
  `CurrentSituation` text,
  `ExpectedResults` text,
  `RootCauses` text,
  `ClosureNotes` text,
  `Status` char(1) DEFAULT NULL COMMENT '{1 = Aperto, 2 = Chiuso}',
  `EndDateExpected` datetime DEFAULT NULL,
  `EndDateReal` datetime DEFAULT NULL,
  `ModifiedBy` varchar(255) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`,`Year`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `improvementactions`
--

LOCK TABLES `improvementactions` WRITE;
/*!40000 ALTER TABLE `improvementactions` DISABLE KEYS */;
/*!40000 ALTER TABLE `improvementactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `improvementactions_team`
--

DROP TABLE IF EXISTS `improvementactions_team`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `improvementactions_team` (
  `ImprovementActionID` int(11) NOT NULL,
  `ImprovementActionYear` year(4) NOT NULL,
  `user` varchar(255) NOT NULL,
  `role` char(1) DEFAULT NULL,
  PRIMARY KEY (`ImprovementActionID`,`ImprovementActionYear`,`user`),
  KEY `Improvement_user_FK` (`user`),
  CONSTRAINT `Improvement_Team_FK` FOREIGN KEY (`ImprovementActionID`, `ImprovementActionYear`) REFERENCES `improvementactions` (`id`, `year`),
  CONSTRAINT `Improvement_user_FK` FOREIGN KEY (`user`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `improvementactions_team`
--

LOCK TABLES `improvementactions_team` WRITE;
/*!40000 ALTER TABLE `improvementactions_team` DISABLE KEYS */;
/*!40000 ALTER TABLE `improvementactions_team` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kpi_description`
--

DROP TABLE IF EXISTS `kpi_description`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `kpi_description` (
  `id` int(11) NOT NULL,
  `name` text NOT NULL,
  `description` text NOT NULL,
  `idprocesso` int(11) NOT NULL,
  `revisione` int(11) NOT NULL,
  `attivo` bit(1) NOT NULL DEFAULT b'1',
  `baseval` float DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `process_FK` (`idprocesso`,`revisione`),
  CONSTRAINT `process_FK` FOREIGN KEY (`idprocesso`, `revisione`) REFERENCES `processo` (`processid`, `revisione`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kpi_description`
--

LOCK TABLES `kpi_description` WRITE;
/*!40000 ALTER TABLE `kpi_description` DISABLE KEYS */;
/*!40000 ALTER TABLE `kpi_description` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kpi_record`
--

DROP TABLE IF EXISTS `kpi_record`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `kpi_record` (
  `kpiID` int(11) NOT NULL,
  `data` datetime NOT NULL,
  `valore` float NOT NULL,
  `task` int(11) DEFAULT NULL,
  PRIMARY KEY (`kpiID`,`data`),
  KEY `kpi_FK` (`kpiID`),
  CONSTRAINT `kpi_FK` FOREIGN KEY (`kpiID`) REFERENCES `kpi_description` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kpi_record`
--

LOCK TABLES `kpi_record` WRITE;
/*!40000 ALTER TABLE `kpi_record` DISABLE KEYS */;
/*!40000 ALTER TABLE `kpi_record` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manuals`
--

DROP TABLE IF EXISTS `manuals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `manuals` (
  `ID` int(11) NOT NULL,
  `Version` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` text,
  `path` varchar(255) DEFAULT NULL,
  `uploaddate` datetime NOT NULL,
  `expirydate` datetime NOT NULL DEFAULT '2199-01-01 00:00:00',
  `isActive` tinyint(4) NOT NULL DEFAULT '1',
  `user` varchar(255) NOT NULL,
  PRIMARY KEY (`ID`,`Version`),
  KEY `user_FK1_idx` (`user`),
  CONSTRAINT `user_FK10` FOREIGN KEY (`user`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manuals`
--

LOCK TABLES `manuals` WRITE;
/*!40000 ALTER TABLE `manuals` DISABLE KEYS */;
/*!40000 ALTER TABLE `manuals` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manualswilabels`
--

DROP TABLE IF EXISTS `manualswilabels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `manualswilabels` (
  `ManualID` int(11) NOT NULL,
  `ManualVersion` int(11) NOT NULL,
  `LabelID` int(11) NOT NULL,
  PRIMARY KEY (`ManualID`,`ManualVersion`,`LabelID`),
  KEY `Label_FK1_idx` (`LabelID`),
  CONSTRAINT `Label_FK1` FOREIGN KEY (`LabelID`) REFERENCES `workinstructionslabel` (`wilabelid`),
  CONSTRAINT `Manual_FK2` FOREIGN KEY (`ManualID`, `ManualVersion`) REFERENCES `manuals` (`id`, `version`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manualswilabels`
--

LOCK TABLES `manualswilabels` WRITE;
/*!40000 ALTER TABLE `manualswilabels` DISABLE KEYS */;
/*!40000 ALTER TABLE `manualswilabels` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `measurementunits`
--

DROP TABLE IF EXISTS `measurementunits`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `measurementunits` (
  `id` int(11) NOT NULL,
  `type` varchar(45) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `isDefault` bit(1) DEFAULT b'0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `type_UNIQUE` (`type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `measurementunits`
--

LOCK TABLES `measurementunits` WRITE;
/*!40000 ALTER TABLE `measurementunits` DISABLE KEYS */;
INSERT INTO `measurementunits` VALUES (0,'UN','Unità',''),(1,'Kg','Chilogrammo','\0'),(2,'mt','Metro','\0'),(3,'h','Ora','\0'),(4,'lt','Litro','\0');
/*!40000 ALTER TABLE `measurementunits` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menualbero`
--

DROP TABLE IF EXISTS `menualbero`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `menualbero` (
  `idPadre` int(11) NOT NULL,
  `idFiglio` int(11) NOT NULL,
  `ordinamento` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`idPadre`,`idFiglio`),
  KEY `menualbero_FK1` (`idPadre`),
  KEY `menualbero_FK2` (`idFiglio`),
  CONSTRAINT `menualbero_FK1` FOREIGN KEY (`idPadre`) REFERENCES `menuvoci` (`id`),
  CONSTRAINT `menualbero_FK2` FOREIGN KEY (`idFiglio`) REFERENCES `menuvoci` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menualbero`
--

LOCK TABLES `menualbero` WRITE;
/*!40000 ALTER TABLE `menualbero` DISABLE KEYS */;
/*!40000 ALTER TABLE `menualbero` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menugruppi`
--

DROP TABLE IF EXISTS `menugruppi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `menugruppi` (
  `gruppo` int(11) NOT NULL,
  `idVoce` int(11) NOT NULL,
  `ordinamento` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`gruppo`,`idVoce`),
  KEY `menugruppi_Fk1` (`gruppo`),
  KEY `menugruppi_FK3` (`idVoce`),
  CONSTRAINT `menugruppi_FK3` FOREIGN KEY (`idVoce`) REFERENCES `menuvoci` (`id`),
  CONSTRAINT `menugruppi_Fk1` FOREIGN KEY (`gruppo`) REFERENCES `groupss` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menugruppi`
--

LOCK TABLES `menugruppi` WRITE;
/*!40000 ALTER TABLE `menugruppi` DISABLE KEYS */;
/*!40000 ALTER TABLE `menugruppi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menuvoci`
--

DROP TABLE IF EXISTS `menuvoci`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `menuvoci` (
  `id` int(11) NOT NULL,
  `titolo` varchar(255) NOT NULL,
  `descrizione` text NOT NULL,
  `url` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menuvoci`
--

LOCK TABLES `menuvoci` WRITE;
/*!40000 ALTER TABLE `menuvoci` DISABLE KEYS */;
INSERT INTO `menuvoci` VALUES (2,'Admin','Admin','~/admin/admin.aspx'),(5,'Product Manager','Process Manager','~/Processi/MacroProcessi.aspx'),(8,'Clienti','Customers menu','~/Commesse/commesse.aspx'),(9,'Production plan','Production plan','~/Produzione/produzione.aspx'),(10,'WorkPlace','WorkPlace','~/Operatori/checkInPostazione.aspx'),(18,'Login','Login','~/Login/login.aspx'),(24,'Nuove commesse','Nuove commesse','~/Produzione/commesseDaProdurre.aspx'),(26,'Barcode Gemba','Interfaccia barcode per operatori via javascript','~/Operatori/AzioniBarcodeJS.aspx'),(28,'Web Gemba','Interfaccia di input operatori completamente via web','~/Workplace/WebGemba/Index'),(29,'Gestione utenti','Gestione utenti','~/Users/manageUsers.aspx'),(30,'Gestione gruppi','Gestione gruppi','~/Users/manageGruppi.aspx'),(31,'Gestione permessi','Gestione permessi','~/Users/managePermessi.aspx'),(32,'Piano Produzione','Piano produttivo completo','~/Produzione/PianoProduzioneCompleto.aspx'),(33,'Storico produzione','Storico produzione','~/Analysis/ProductionHistory/Index'),(34,'Andon','Andon','~/Produzione/AndonListReparti.aspx'),(35,'Andon Reparto','Andon Reparto','~/Produzione/AndonListReparti.aspx'),(36,'Andon Generale','Andon Generale','~/Produzione/avanzamentoProduzione.aspx'),(37,'Gestione menu','Gestione menu','~/Admin/manageMenu.aspx'),(39,'Carico di lavoro','Gestione e simulazione carico di lavoro per reparto','~/Analysis/ProductionWorkload/Index'),(41,'Configurazione principale','Menu di configurazione del sistema generale','~/Admin/kisAdmin.aspx'),(42,'Gestione Andon Completo','Gestione Andon Completo','~/Andon/configAndonCompleto.aspx'),(43,'Anagrafica clienti','Anagrafica clienti','~/Clienti/clienti.aspx'),(44,'Ordini','Ordini','~/Commesse/commesse.aspx'),(45,'Wizard nuovo ordine','Wizard prodotto su commessa	','~/Commesse/wzAddCommessa.aspx'),(46,'Analisi dati','Analisi dati','~/Analysis/Analysis.aspx'),(47,'myArea','Gestione account personale','~/Personal/my.aspx'),(48,'Configurazione Reports','Configurazione reports','~/Admin/manageReports.aspx'),(49,'Configurazione Reparti','Gestione reparti','~/Reparti/listReparti.aspx'),(50,'Configurazione Postazioni di Lavoro','Configurazione Postazioni di Lavoro','~/Postazioni/managePostazioniLavoro.aspx'),(51,'Qualità','Gestione qualità','~/Quality/Home/Index'),(52,'Unita di misura','Gestione unita di misura','~/Config/MeasurementUnits/Index'),(62,'Task Non Produttivi','Task non produttivi','~/Config/NoProductiveTasks/Index'),(63,'Process Manager','Process Manager','~/Processi/MacroProcessi.aspx'),(64,'Work Instructions','Work Instructions','~/WorkInstructions/WorkInstructions/Index'),(65,'Free Measurements','Free Measurements','~/FreeTimeMeasurement/FreeMeasurement/ChooseDepartment'),(66,'Free Measurements','Free Measurements','~/FreeTimeMeasurement/FreeMeasurement/Index'),(67,'FreeMeasurements','FreeMeasurements','~/FreeTimeMeasurement/FreeMeasurement/Index'),(68,'Task non produttivi','Task non produttivi','~/Config/NoProductiveTasks/Index');
/*!40000 ALTER TABLE `menuvoci` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `microsteps`
--

DROP TABLE IF EXISTS `microsteps`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `microsteps` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `review` int(11) NOT NULL DEFAULT '0',
  `name` varchar(255) NOT NULL,
  `description` text,
  `creation_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`,`review`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `microsteps`
--

LOCK TABLES `microsteps` WRITE;
/*!40000 ALTER TABLE `microsteps` DISABLE KEYS */;
INSERT INTO `microsteps` VALUES (1,0,'Image','https://www.cbronline.com/wp-content/uploads/2016/06/what-is-URL-770x503.jpg','2021-02-26 00:00:00');
/*!40000 ALTER TABLE `microsteps` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `microsteps_value`
--

DROP TABLE IF EXISTS `microsteps_value`;
/*!50001 DROP VIEW IF EXISTS `microsteps_value`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `microsteps_value` AS SELECT 
 1 AS `Macro_ProcessID`,
 1 AS `revisione`,
 1 AS `variante`,
 1 AS `Macro_Name`,
 1 AS `Macro_Description`,
 1 AS `num_op`,
 1 AS `setup`,
 1 AS `tempo`,
 1 AS `tunload`,
 1 AS `Micro_ProcessID`,
 1 AS `Micro_Name`,
 1 AS `Micro_Description`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `model_microsteps`
--

DROP TABLE IF EXISTS `model_microsteps`;
/*!50001 DROP VIEW IF EXISTS `model_microsteps`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `model_microsteps` AS SELECT 
 1 AS `ProductLine_Name`,
 1 AS `Product_Name`,
 1 AS `Macrostep_ProcessID`,
 1 AS `revisione`,
 1 AS `variante`,
 1 AS `Macrostep_Name`,
 1 AS `Macro_Description`,
 1 AS `num_op`,
 1 AS `setup`,
 1 AS `tempo`,
 1 AS `tunload`,
 1 AS `Microstep_Name`,
 1 AS `Microstep_Description`,
 1 AS `Microstep_Sequence`,
 1 AS `Microstep_CycleTime`,
 1 AS `Microstep_ValueOrWaste`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `model_microsteps_concat`
--

DROP TABLE IF EXISTS `model_microsteps_concat`;
/*!50001 DROP VIEW IF EXISTS `model_microsteps_concat`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `model_microsteps_concat` AS SELECT 
 1 AS `ProductLine_Name`,
 1 AS `Product_Name`,
 1 AS `Macro_ProcessID`,
 1 AS `Macro_Review`,
 1 AS `Macro_Product`,
 1 AS `Macro_Name`,
 1 AS `Macro_Description`,
 1 AS `num_op`,
 1 AS `setup`,
 1 AS `tempo`,
 1 AS `tunload`,
 1 AS `Microsteps_Name`,
 1 AS `Microstep_CycleTime`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `modelparameters`
--

DROP TABLE IF EXISTS `modelparameters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `modelparameters` (
  `processID` int(11) NOT NULL,
  `processRev` int(11) NOT NULL,
  `varianteID` int(11) NOT NULL,
  `paramID` int(11) NOT NULL,
  `paramCategory` int(11) NOT NULL,
  `paramName` varchar(255) DEFAULT NULL,
  `paramDescription` text,
  `isFixed` bit(1) DEFAULT NULL,
  `isRequired` bit(1) DEFAULT NULL,
  `sequence` int(11) NOT NULL,
  PRIMARY KEY (`processID`,`processRev`,`varianteID`,`paramID`),
  KEY `paramCategory` (`paramCategory`),
  CONSTRAINT `modelparameters_ibfk_1` FOREIGN KEY (`processID`, `processRev`, `varianteID`) REFERENCES `variantiprocessi` (`processo`, `revproc`, `variante`),
  CONSTRAINT `modelparameters_ibfk_2` FOREIGN KEY (`paramCategory`) REFERENCES `productparameterscategories` (`paramcatid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `modelparameters`
--

LOCK TABLES `modelparameters` WRITE;
/*!40000 ALTER TABLE `modelparameters` DISABLE KEYS */;
/*!40000 ALTER TABLE `modelparameters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `modeltaskparameters`
--

DROP TABLE IF EXISTS `modeltaskparameters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `modeltaskparameters` (
  `taskID` int(11) NOT NULL,
  `taskRev` int(11) NOT NULL,
  `varianteID` int(11) NOT NULL,
  `paramID` int(11) NOT NULL,
  `paramCategory` int(11) NOT NULL,
  `paramName` varchar(255) DEFAULT NULL,
  `paramDescription` text,
  `isFixed` bit(1) DEFAULT NULL,
  `isRequired` bit(1) DEFAULT NULL,
  `sequence` int(11) NOT NULL,
  PRIMARY KEY (`taskID`,`taskRev`,`varianteID`,`paramID`),
  KEY `paramCategory` (`paramCategory`),
  CONSTRAINT `modeltaskparameters_ibfk_1` FOREIGN KEY (`taskID`, `taskRev`, `varianteID`) REFERENCES `processipadrifigli` (`task`, `revtask`, `variante`),
  CONSTRAINT `modeltaskparameters_ibfk_2` FOREIGN KEY (`paramCategory`) REFERENCES `productparameterscategories` (`paramcatid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `modeltaskparameters`
--

LOCK TABLES `modeltaskparameters` WRITE;
/*!40000 ALTER TABLE `modeltaskparameters` DISABLE KEYS */;
/*!40000 ALTER TABLE `modeltaskparameters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `noncompliances`
--

DROP TABLE IF EXISTS `noncompliances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `noncompliances` (
  `ID` int(11) NOT NULL,
  `Year` year(4) NOT NULL,
  `Quantity` int(11) DEFAULT NULL,
  `OpeningDate` datetime NOT NULL,
  `User` varchar(255) NOT NULL COMMENT 'Utente che ha rilevato la non conformita'' (se Warning = utente che ha sparato, altrimenti utente che apre la nc)',
  `Description` text,
  `ImmediateAction` text,
  `Cost` double DEFAULT NULL,
  `Status` char(1) NOT NULL COMMENT '{O = aperta, C = chiusa}',
  `ClosureDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`,`Year`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `noncompliances`
--

LOCK TABLES `noncompliances` WRITE;
/*!40000 ALTER TABLE `noncompliances` DISABLE KEYS */;
/*!40000 ALTER TABLE `noncompliances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `noncompliances_products`
--

DROP TABLE IF EXISTS `noncompliances_products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `noncompliances_products` (
  `NonComplianceID` int(11) NOT NULL,
  `NonComplianceYear` year(4) NOT NULL,
  `ProductID` int(11) NOT NULL,
  `ProductYear` year(4) NOT NULL,
  `Source` char(1) NOT NULL,
  `WarningID` int(11) DEFAULT NULL,
  `Workstation` int(11) DEFAULT NULL COMMENT 'Se Source = Warning, e'' la ostazione in cui e'' stata rilevata la non conformita''.\n-1 altrimenti',
  `Quantity` int(11) NOT NULL,
  PRIMARY KEY (`NonComplianceID`,`NonComplianceYear`,`ProductID`,`ProductYear`),
  KEY `NC_Products_Prod_FK` (`ProductID`,`ProductYear`),
  KEY `NC_Products_Postazione_FK` (`Workstation`),
  KEY `NC_Products_Warning_FK` (`WarningID`),
  CONSTRAINT `NC_Products_NC_FK` FOREIGN KEY (`NonComplianceID`, `NonComplianceYear`) REFERENCES `noncompliances` (`id`, `year`),
  CONSTRAINT `NC_Products_Postazione_FK` FOREIGN KEY (`Workstation`) REFERENCES `postazioni` (`idpostazioni`),
  CONSTRAINT `NC_Products_Prod_FK` FOREIGN KEY (`ProductID`, `ProductYear`) REFERENCES `productionplan` (`id`, `anno`),
  CONSTRAINT `NC_Products_Warning_FK` FOREIGN KEY (`WarningID`) REFERENCES `warningproduzione` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `noncompliances_products`
--

LOCK TABLES `noncompliances_products` WRITE;
/*!40000 ALTER TABLE `noncompliances_products` DISABLE KEYS */;
/*!40000 ALTER TABLE `noncompliances_products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `noncompliancescause`
--

DROP TABLE IF EXISTS `noncompliancescause`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `noncompliancescause` (
  `ID` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` text,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `noncompliancescause`
--

LOCK TABLES `noncompliancescause` WRITE;
/*!40000 ALTER TABLE `noncompliancescause` DISABLE KEYS */;
/*!40000 ALTER TABLE `noncompliancescause` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `noncompliancescause_nc`
--

DROP TABLE IF EXISTS `noncompliancescause_nc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `noncompliancescause_nc` (
  `CauseID` int(11) NOT NULL,
  `NCID` int(11) NOT NULL,
  `NCYear` year(4) NOT NULL,
  PRIMARY KEY (`CauseID`,`NCID`,`NCYear`),
  KEY `NonCompliances_Cause_FK_idx` (`NCID`,`NCYear`),
  CONSTRAINT `NonCompliances_Cause1_FK` FOREIGN KEY (`CauseID`) REFERENCES `noncompliancescause` (`id`),
  CONSTRAINT `NonCompliances_Cause_FK` FOREIGN KEY (`NCID`, `NCYear`) REFERENCES `noncompliances` (`id`, `year`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `noncompliancescause_nc`
--

LOCK TABLES `noncompliancescause_nc` WRITE;
/*!40000 ALTER TABLE `noncompliancescause_nc` DISABLE KEYS */;
/*!40000 ALTER TABLE `noncompliancescause_nc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `noncompliancestype_nc`
--

DROP TABLE IF EXISTS `noncompliancestype_nc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `noncompliancestype_nc` (
  `TypeID` int(11) NOT NULL,
  `NCID` int(11) NOT NULL,
  `NCYear` year(4) NOT NULL,
  PRIMARY KEY (`TypeID`,`NCID`,`NCYear`),
  KEY `NonCompliance_FK_idx` (`NCID`,`NCYear`),
  CONSTRAINT `NonCompliance_FK` FOREIGN KEY (`NCID`, `NCYear`) REFERENCES `noncompliances` (`id`, `year`),
  CONSTRAINT `Type_FK` FOREIGN KEY (`TypeID`) REFERENCES `noncompliancestypes` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `noncompliancestype_nc`
--

LOCK TABLES `noncompliancestype_nc` WRITE;
/*!40000 ALTER TABLE `noncompliancestype_nc` DISABLE KEYS */;
/*!40000 ALTER TABLE `noncompliancestype_nc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `noncompliancestypes`
--

DROP TABLE IF EXISTS `noncompliancestypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `noncompliancestypes` (
  `ID` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` text,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `noncompliancestypes`
--

LOCK TABLES `noncompliancestypes` WRITE;
/*!40000 ALTER TABLE `noncompliancestypes` DISABLE KEYS */;
/*!40000 ALTER TABLE `noncompliancestypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `noproductivetasks`
--

DROP TABLE IF EXISTS `noproductivetasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `noproductivetasks` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `description` text,
  `enabled` bit(1) NOT NULL DEFAULT b'1',
  `creationdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `isdefault` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `noproductivetasks`
--

LOCK TABLES `noproductivetasks` WRITE;
/*!40000 ALTER TABLE `noproductivetasks` DISABLE KEYS */;
INSERT INTO `noproductivetasks` VALUES (1,'Pulizia e ordine','','','2021-02-18 18:10:29','\0'),(2,'Fine giornata lavorativa','Selezionare questa attività per terminare la giornata lavorativa','','2021-02-18 18:10:55','\0'),(3,'Attesa','Attesa di un nuovo lavoro da fare','','2021-02-18 18:11:11',''),(4,'Pausa pranzo/Caffè','','','2021-02-18 18:12:26','\0');
/*!40000 ALTER TABLE `noproductivetasks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operatorireparto`
--

DROP TABLE IF EXISTS `operatorireparto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `operatorireparto` (
  `operatore` varchar(255) NOT NULL,
  `reparto` int(11) NOT NULL,
  PRIMARY KEY (`operatore`,`reparto`),
  KEY `FK_operatore` (`operatore`),
  KEY `FK_reparto` (`reparto`),
  CONSTRAINT `FK_operatore` FOREIGN KEY (`operatore`) REFERENCES `users` (`userid`),
  CONSTRAINT `FK_reparto` FOREIGN KEY (`reparto`) REFERENCES `reparti` (`idreparto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operatorireparto`
--

LOCK TABLES `operatorireparto` WRITE;
/*!40000 ALTER TABLE `operatorireparto` DISABLE KEYS */;
/*!40000 ALTER TABLE `operatorireparto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orarilavoroturni`
--

DROP TABLE IF EXISTS `orarilavoroturni`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `orarilavoroturni` (
  `id` int(11) NOT NULL,
  `idTurno` int(11) NOT NULL,
  `giornoInizio` int(11) NOT NULL,
  `oraInizio` time NOT NULL,
  `giornoFine` int(11) NOT NULL,
  `oraFine` time NOT NULL,
  PRIMARY KEY (`id`),
  KEY `orariLavoroTurni_FK1` (`idTurno`),
  CONSTRAINT `orariLavoroTurni_FK1` FOREIGN KEY (`idTurno`) REFERENCES `turniproduzione` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orarilavoroturni`
--

LOCK TABLES `orarilavoroturni` WRITE;
/*!40000 ALTER TABLE `orarilavoroturni` DISABLE KEYS */;
INSERT INTO `orarilavoroturni` VALUES (0,0,1,'08:00:00',1,'12:00:00'),(1,0,2,'08:00:00',2,'12:00:00'),(2,0,3,'08:00:00',3,'12:00:00'),(3,0,4,'08:00:00',4,'12:00:00'),(4,0,5,'08:00:00',5,'12:00:00');
/*!40000 ALTER TABLE `orarilavoroturni` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permessi`
--

DROP TABLE IF EXISTS `permessi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `permessi` (
  `idpermesso` int(11) NOT NULL,
  `nome` varchar(45) DEFAULT NULL,
  `descrizione` text,
  PRIMARY KEY (`idpermesso`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permessi`
--

LOCK TABLES `permessi` WRITE;
/*!40000 ALTER TABLE `permessi` DISABLE KEYS */;
INSERT INTO `permessi` VALUES (4,'Postazione check-in','Permesso di eseguire il check-in su una postazione di lavoro'),(5,'Task Produzione','Visione, gestione, esecuzione task di produzione'),(6,'Utenti','Gestione utenti'),(7,'Gruppi','Gestione gruppi'),(8,'Gruppi Permessi','Associazione dei permessi ai gruppi'),(9,'Menu Voce','Gestione delle voci di menu'),(10,'Gruppi Menu','Gestisce l&#39;associazione delle voci di menu ai gruppi'),(11,'Commesse','Gestione commesse'),(12,'Articoli','Gestione articoli in produzione'),(13,'Postazione','Gestione postazioni'),(14,'Calendario Postazione','Gestisce il calendario di postazione'),(15,'Postazioni Operatori','Gestisce gli operatori in postazione'),(16,'Processo','Gestione processi'),(17,'Reparto ProcessoVariante','Gestione Reparti - ProcessoVariante'),(18,'Processo TempiCiclo','Gestione tempi ciclo nei processi'),(19,'Processo Variante','Gestione dei Processo Variante'),(20,'Reparto','Gestione reparti'),(21,'Warning','GEstione degli warning'),(22,'Turno','Gestione turni di lavoro'),(23,'Reparto Operatori','Gestione operatori di reparto.'),(24,'Reparto Straordinari','Gestione straordinari di reparto.'),(25,'Reparto Festivita','Gestione festivit&#224; di reparto.'),(26,'Permessi','Gestione permessi'),(27,'Task Postazione','Associazione dei tasks alle postazioni di lavoro'),(28,'Reparto WorkLoad','Gestione carico di lavoro di un reparto'),(29,'Utenti E-mail','Gestione indirizzi e-mail degli utenti'),(30,'Utenti PhoneNumbers','Gestione dei numeri telefonici degli utenti'),(31,'Reparto EventoRitardo','Gestione ritardi per reparto'),(32,'Reparto EventoWarning','Gestione eventi warning all&#39;interno dei reparti'),(33,'Commessa EventoRitardo','Gestione della configurazione dei ritardi per la commessa'),(34,'Commessa EventoWarning','Gestione degli eventi warning di commessa'),(35,'Articolo EventoRitardo','Gestione configurazione eventi di ritardo per articolo'),(36,'Articolo EventoWarning','Gestione configurazione eventi segnalazione di warning per articolo'),(37,'Reparto ModoCalcoloTC','Impostare il modo di calcolo del tempo ciclo'),(38,'Articolo Depianifica','Rimuovere un prodotto dal piano produzione'),(39,'Configurazione Logo','Configurazione Logo'),(40,'Reparto Andon VisualizzazioneNomiUtente','Configurazione visualizzazione nomi su andon'),(41,'AndonCompleto VisualizzazioneNomiUtente','Configurazione visualizzazione nomi utente su andon'),(42,'Reparto AvvioTasksOperatori','Gestione del numero massimo di tasks avviabili dagli operatori'),(43,'Anagrafica Clienti','Anagrafica Clienti'),(44,'Andon Reparto VisualizzazioneGiorni','Andon Reparto VisualizzazioneGiorni'),(45,'Andon Reparto','Visualizzazione andon reparto'),(46,'Anagrafica Clienti Contatti','Anagrafica Clienti Contatti'),(47,'Analisi Commessa Costo','Analisi Commessa Costo'),(48,'Analisi Operatori Tempi','Analisi Operatori Tempi'),(49,'Analisi','Interfaccia analisi dati'),(50,'Analisi Articolo Costo','Analisi Articolo Costo'),(51,'Analisi TipoProdotto','Accesso all&#39;interfaccia di analisi per tipo di prodotto'),(52,'Analisi Tasks','Accesso all&#39;interfaccia di analisi dei tasks'),(53,'Wizard TipoPERT','Permesso di variare la tipologia di visualizzazione della creazione processi, da PERT a Tabella e viceversa'),(54,'Analisi Clienti','Permesso per visualizzare interfaccia analisi tempi di lavoro clienti'),(55,'Report Stato Ordini Clienti','Report Stato Ordini Clienti'),(56,'Configurazione Report Stato Ordini Clienti','Configurazione Report Stato Ordini Clienti'),(57,'Turno PostazioneRisorse','Gestione della capacit&#224; produttiva per turno'),(58,'AndonCompleto CampiDaVisualizzare','Gestione campi visualizzati su Andon'),(59,'AndonReparto CampiDaVisualizzare','Gestione campi da visualizzare su Andon Reparto'),(60,'Configurazione TimeZone','Permette di configurare il fuso orario'),(61,'Reparto Timezone','Permette di configurare il fuso orario per singolo reparto'),(62,'Articolo Riesuma','Articolo Riesuma'),(63,'TaskProduzione Riesuma','TaskProduzione Riesuma'),(64,'Reparto ConfigurazioneKanban','Abilitazione KanbanBox by Sintesia'),(65,'NonCompliance Types','Categorie di non conformita'),(66,'NonCompliance Causes','Cause di non conformita'),(67,'NonCompliances','Non conformita'),(68,'NonCompliancesAnalysis Product','NonCompliancesAnalysis Product'),(69,'NonCompliancesAnalysis Num','NonCompliancesAnalysis Num'),(70,'NonCompliancesAnalysis Cost','NonCompliancesAnalysis Cost'),(71,'ImprovementActions','ImprovementActions'),(72,'Billing','Billing service'),(73,'Product ParameterCategories','Product ParameterCategories'),(74,'Product Parameters','Product Parameters'),(75,'Customer Order','Permission given to the customer to make an online order'),(76,'Task Parameter','Set task parameter\'s value'),(84,'AndonCompleto ScrollType','AndonCompleto ScrollType'),(85,'Working Hours Manual Registration','Working Hours Manual Registration'),(86,'Config SalesOrdersImport3rdPartySystem','Config SalesOrdersImport3rdPartySystem'),(87,'Config MeasurementUnits','Measurement Units management authorization'),(88,'WorkInstructions Manage','WorkInstructions Manage'),(89,'Task WorkInstructions','Task WorkInstructions'),(93,'Task DefaultOperators','Task DefaultOperators'),(94,'Tasks ManageOperators','Tasks ManageOperators'),(97,'Tasks ManuallyReschedule','Tasks ManuallyReschedule'),(101,'Task Microsteps','Task Microsteps'),(102,'Config NoProductiveTasks','Config NoProductiveTasks'),(103,'FreeMeasurement Manage','FreeMeasurement Manage'),(104,'FreeMeasurement ExecuteTasks','FreeMeasurement ExecuteTasks');
/*!40000 ALTER TABLE `permessi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `postazioni`
--

DROP TABLE IF EXISTS `postazioni`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `postazioni` (
  `idpostazioni` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text,
  `barcodeAutoCheckIn` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`idpostazioni`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `postazioni`
--

LOCK TABLES `postazioni` WRITE;
/*!40000 ALTER TABLE `postazioni` DISABLE KEYS */;
INSERT INTO `postazioni` VALUES (20,'Spedizione','Postazione spedizione','\0'),(21,'Produzione','Postazione produzione prodotto principale','\0'),(22,'Magazzino picking','Area di picking',''),(23,'Taglio','Postazione di taglio',''),(24,'New','qwerty','\0');
/*!40000 ALTER TABLE `postazioni` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `precedenzeprocessi`
--

DROP TABLE IF EXISTS `precedenzeprocessi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `precedenzeprocessi` (
  `prec` int(11) NOT NULL,
  `revPrec` int(11) NOT NULL,
  `succ` int(11) NOT NULL,
  `revSucc` int(11) NOT NULL,
  `variante` int(11) NOT NULL,
  `relazione` int(11) NOT NULL,
  `pausa` time DEFAULT '00:00:00',
  `ConstraintType` int(11) DEFAULT '0',
  PRIMARY KEY (`prec`,`revPrec`,`succ`,`revSucc`,`variante`),
  KEY `precedenzeprocessi_ibfk_3` (`relazione`),
  KEY `precedenzeprocessi_ibfk_2` (`succ`,`revSucc`),
  KEY `precedenzeprocessi_variante` (`variante`),
  KEY `precedenzeprocessi_ibfk_4` (`variante`),
  CONSTRAINT `precedenzeprocessi_ibfk_1` FOREIGN KEY (`prec`, `revPrec`) REFERENCES `processo` (`processid`, `revisione`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `precedenzeprocessi_ibfk_2` FOREIGN KEY (`succ`, `revSucc`) REFERENCES `processo` (`processid`, `revisione`) ON UPDATE CASCADE,
  CONSTRAINT `precedenzeprocessi_ibfk_3` FOREIGN KEY (`relazione`) REFERENCES `relazioniprocessi` (`relazioneid`) ON UPDATE CASCADE,
  CONSTRAINT `precedenzeprocessi_ibfk_4` FOREIGN KEY (`variante`) REFERENCES `varianti` (`idvariante`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `precedenzeprocessi`
--

LOCK TABLES `precedenzeprocessi` WRITE;
/*!40000 ALTER TABLE `precedenzeprocessi` DISABLE KEYS */;
INSERT INTO `precedenzeprocessi` VALUES (4,0,5,0,2,0,'00:00:00',0),(6,0,7,0,4,0,'00:00:00',0),(6,0,9,0,5,0,'00:00:00',0),(6,0,15,0,3,0,'00:00:00',0),(7,0,5,0,7,0,'00:00:00',0),(7,0,8,0,3,0,'00:00:00',0),(7,0,8,0,4,0,'00:00:00',0),(7,0,8,0,5,0,'00:00:00',0),(8,0,9,0,4,0,'00:00:00',0),(8,0,14,0,3,0,'00:00:00',0),(8,0,14,0,5,0,'00:00:00',0),(10,0,11,0,3,0,'00:00:00',0),(10,0,12,0,3,0,'00:00:00',0),(11,0,13,0,3,0,'00:00:00',0),(12,0,13,0,3,0,'00:00:00',0),(13,0,14,0,3,0,'00:00:00',0),(14,0,9,0,5,0,'00:00:00',0),(14,0,15,0,3,0,'00:00:00',0),(15,0,9,0,3,0,'00:00:00',0);
/*!40000 ALTER TABLE `precedenzeprocessi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prectasksproduzione`
--

DROP TABLE IF EXISTS `prectasksproduzione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `prectasksproduzione` (
  `prec` int(11) NOT NULL,
  `succ` int(11) NOT NULL,
  `relazione` int(11) NOT NULL DEFAULT '0',
  `pausa` time DEFAULT '00:00:00',
  `ConstraintType` int(11) DEFAULT '0',
  PRIMARY KEY (`prec`,`succ`),
  KEY `prectasksproduzione_FK_prec` (`prec`),
  KEY `prectasksproduzione_FK_succ` (`succ`),
  CONSTRAINT `prectasksproduzione_FK_prec` FOREIGN KEY (`prec`) REFERENCES `tasksproduzione` (`taskid`) ON UPDATE CASCADE,
  CONSTRAINT `prectasksproduzione_FK_succ` FOREIGN KEY (`succ`) REFERENCES `tasksproduzione` (`taskid`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prectasksproduzione`
--

LOCK TABLES `prectasksproduzione` WRITE;
/*!40000 ALTER TABLE `prectasksproduzione` DISABLE KEYS */;
INSERT INTO `prectasksproduzione` VALUES (0,3,0,'00:00:00',0),(1,2,0,'00:00:00',0),(2,4,0,'00:00:00',0),(4,3,0,'00:00:00',0);
/*!40000 ALTER TABLE `prectasksproduzione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `processipadrifigli`
--

DROP TABLE IF EXISTS `processipadrifigli`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `processipadrifigli` (
  `task` int(11) NOT NULL,
  `revTask` int(11) NOT NULL,
  `padre` int(11) NOT NULL,
  `revPadre` int(11) NOT NULL,
  `variante` int(11) NOT NULL,
  `posx` int(11) NOT NULL DEFAULT '100',
  `posy` int(11) NOT NULL DEFAULT '100',
  PRIMARY KEY (`task`,`variante`,`revPadre`,`padre`,`revTask`),
  KEY `FK_Task` (`task`,`revTask`),
  KEY `FK_PadreVariante` (`padre`,`revPadre`,`variante`),
  CONSTRAINT `FK_PadreVariante` FOREIGN KEY (`padre`, `revPadre`, `variante`) REFERENCES `variantiprocessi` (`processo`, `revproc`, `variante`),
  CONSTRAINT `FK_Task` FOREIGN KEY (`padre`, `revPadre`, `variante`) REFERENCES `variantiprocessi` (`processo`, `revproc`, `variante`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `processipadrifigli`
--

LOCK TABLES `processipadrifigli` WRITE;
/*!40000 ALTER TABLE `processipadrifigli` DISABLE KEYS */;
INSERT INTO `processipadrifigli` VALUES (4,0,3,0,2,138,160),(5,0,3,0,2,417,215),(5,0,1,0,7,584,198),(6,0,1,0,3,586,500),(6,0,0,0,4,138,221),(6,0,16,0,5,174,156),(7,0,1,0,3,117,384),(7,0,0,0,4,368,226),(7,0,16,0,5,133,328),(7,0,1,0,7,216,219),(8,0,1,0,3,448,384),(8,0,0,0,4,571,247),(8,0,16,0,5,336,350),(9,0,1,0,3,1061,355),(9,0,0,0,4,782,226),(9,0,16,0,5,772,214),(10,0,1,0,3,90,116),(11,0,1,0,3,320,90),(12,0,1,0,3,332,230),(13,0,1,0,3,585,213),(14,0,1,0,3,801,229),(14,0,16,0,5,530,311),(15,0,1,0,3,914,363),(17,0,2,0,6,221,207),(18,0,2,0,6,223,348),(19,0,2,0,6,213,496),(20,0,2,0,6,227,82);
/*!40000 ALTER TABLE `processipadrifigli` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `processo`
--

DROP TABLE IF EXISTS `processo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `processo` (
  `ProcessID` int(11) NOT NULL,
  `revisione` int(11) NOT NULL,
  `dataRevisione` datetime NOT NULL,
  `Name` text NOT NULL,
  `Description` text,
  `isVSM` bit(1) NOT NULL,
  `posx` int(11) NOT NULL,
  `posy` int(11) NOT NULL,
  `attivo` bit(1) NOT NULL,
  PRIMARY KEY (`ProcessID`,`revisione`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `processo`
--

LOCK TABLES `processo` WRITE;
/*!40000 ALTER TABLE `processo` DISABLE KEYS */;
INSERT INTO `processo` VALUES (0,0,'2021-02-18 18:01:10','Kit Porte','Kit Porte','\0',0,0,''),(1,0,'2021-02-18 18:01:22','Porte','Porte','\0',0,0,''),(2,0,'2021-02-18 18:01:33','Accessori maniglie','Assemblaggio Accessori &quot;maniglie&quot;','\0',0,0,''),(3,0,'2021-02-18 18:01:42','Spedizioni','Spedizioni','\0',0,0,''),(4,0,'2021-02-18 18:15:08','Imballaggio','New Default Process Notes','\0',100,100,''),(5,0,'2021-02-18 18:15:32','Carico camion','https://www.cbronline.com/wp-content/uploads/2016/06/what-is-URL-770x503.jpg','\0',100,100,''),(6,0,'2021-02-18 20:06:04','Prelievo accessori','Prelievo accessori (tutti i componeti necessari per l&#39;assemblaggio finale della porta)','\0',100,100,''),(7,0,'2021-02-22 11:37:47','Prelievo profili','Prelievo profili in PVC e Alluminio per il taglio','\0',100,100,''),(8,0,'2021-02-22 11:38:15','Taglio profili a misura','Taglio profili a misura PVC e Alluminio','\0',100,100,''),(9,0,'2021-02-22 11:38:42','Preparazione per imballaggio','Preparazione per imballaggio','\0',100,100,''),(10,0,'2021-02-22 11:43:54','Prelievo Pannelli porta','Prelievo Pannelli porta','\0',100,100,''),(11,0,'2021-02-22 11:44:22','Taglio pannelli porta su macchina taglio','Taglio pannelli porta su macchina taglio','\0',100,100,''),(12,0,'2021-02-22 11:46:01','Taglio pannelli porta MANUALE','Taglio pannelli porta manuale se profile &amp;gt; 1000mm','\0',100,100,''),(13,0,'2021-02-22 11:46:41','Scanalatura pannelli per assemblaggio','Scanalatura pannelli per assemblaggio','\0',100,100,''),(14,0,'2021-02-22 11:47:39','Inserimento rinforzo su profili e saldatura','Inserimento rinforzo su profili e saldatura profili','\0',100,100,''),(15,0,'2021-02-22 11:48:48','Assemblaggio','New Default Process Notes','\0',100,100,''),(16,0,'2021-02-22 13:06:54','Ante','Ante','\0',0,0,''),(17,0,'2021-02-22 13:16:32','Assemblaggio carrucole','Assemblaggio carrucole','\0',100,100,''),(18,0,'2021-02-22 13:17:10','Assemblaggio Perno','Assemblaggio Perno (Perno - Rondella - Dadi)','\0',100,100,''),(19,0,'2021-02-22 13:17:43','Assemblaggio cuscinetto e ruota su perno','Assemblaggio cuscinetto e ruota su perno','\0',100,100,''),(20,0,'2021-02-22 13:18:08','Assemblaggio ANIMA perno su blocchetto ','Assemblaggio ANIMA perno su blocchetto ','\0',100,100,'');
/*!40000 ALTER TABLE `processo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `processowners`
--

DROP TABLE IF EXISTS `processowners`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `processowners` (
  `user` varchar(255) NOT NULL,
  `process` int(11) NOT NULL,
  `revProc` int(11) NOT NULL,
  PRIMARY KEY (`user`,`process`,`revProc`),
  KEY `process` (`process`,`revProc`),
  CONSTRAINT `processowners_ibfk_1` FOREIGN KEY (`user`) REFERENCES `users` (`userid`) ON UPDATE CASCADE,
  CONSTRAINT `processowners_ibfk_2` FOREIGN KEY (`process`, `revProc`) REFERENCES `processo` (`processid`, `revisione`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `processowners`
--

LOCK TABLES `processowners` WRITE;
/*!40000 ALTER TABLE `processowners` DISABLE KEYS */;
/*!40000 ALTER TABLE `processowners` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productionplan`
--

DROP TABLE IF EXISTS `productionplan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `productionplan` (
  `id` int(11) NOT NULL,
  `anno` year(4) NOT NULL,
  `processo` int(11) NOT NULL,
  `revisione` int(11) NOT NULL,
  `variante` int(11) NOT NULL,
  `matricola` varchar(255) DEFAULT NULL,
  `status` varchar(1) NOT NULL,
  `reparto` int(11) DEFAULT NULL,
  `startTime` datetime DEFAULT NULL,
  `commessa` int(11) NOT NULL,
  `annoCommessa` year(4) NOT NULL,
  `dataConsegnaPrevista` datetime NOT NULL,
  `dataPrevistaFineProduzione` datetime DEFAULT NULL,
  `planner` varchar(255) DEFAULT NULL,
  `quantita` int(11) NOT NULL DEFAULT '1',
  `quantitaProdotta` int(11) DEFAULT NULL,
  `measurementUnit` int(11) DEFAULT '0',
  `kanbanCard` varchar(255) DEFAULT NULL,
  `LeadTime` time NOT NULL DEFAULT '00:00:00',
  `WorkingTimePlanned` time NOT NULL DEFAULT '00:00:00',
  `WorkingTime` time NOT NULL DEFAULT '00:00:00',
  `Delay` time NOT NULL DEFAULT '00:00:00',
  `EndProductionDateReal` datetime DEFAULT NULL,
  PRIMARY KEY (`id`,`anno`),
  KEY `productionplan_ibfk_3` (`reparto`),
  KEY `commesse_FK1` (`commessa`,`annoCommessa`),
  KEY `processo_FK` (`processo`,`revisione`,`variante`),
  KEY `reparto_FK1` (`reparto`),
  KEY `productionplan_FKplanner_idx` (`planner`),
  KEY `measurementUnit_FK1_idx` (`measurementUnit`),
  CONSTRAINT `commesse_FK1` FOREIGN KEY (`commessa`, `annoCommessa`) REFERENCES `commesse` (`idcommesse`, `anno`),
  CONSTRAINT `measurementUnit_FK2` FOREIGN KEY (`measurementUnit`) REFERENCES `measurementunits` (`id`),
  CONSTRAINT `measurementUnit_FK3` FOREIGN KEY (`measurementUnit`) REFERENCES `measurementunits` (`id`),
  CONSTRAINT `processo_FK` FOREIGN KEY (`processo`, `revisione`, `variante`) REFERENCES `variantiprocessi` (`processo`, `revproc`, `variante`),
  CONSTRAINT `productionplan_FKplanner` FOREIGN KEY (`planner`) REFERENCES `users` (`userid`),
  CONSTRAINT `reparto_FK1` FOREIGN KEY (`reparto`) REFERENCES `reparti` (`idreparto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productionplan`
--

LOCK TABLES `productionplan` WRITE;
/*!40000 ALTER TABLE `productionplan` DISABLE KEYS */;
INSERT INTO `productionplan` VALUES (0,2021,16,0,5,NULL,'P',0,NULL,0,2021,'2021-04-29 22:00:00','2021-04-29 22:00:00',NULL,4,4,0,NULL,'00:00:00','20:00:00','00:00:00','00:00:00',NULL);
/*!40000 ALTER TABLE `productionplan` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productparameters`
--

DROP TABLE IF EXISTS `productparameters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `productparameters` (
  `productID` int(11) NOT NULL,
  `productYear` year(4) NOT NULL,
  `paramID` int(11) NOT NULL,
  `paramCategory` int(11) NOT NULL,
  `paramName` varchar(255) DEFAULT NULL,
  `paramDescription` text,
  `isFixed` bit(1) DEFAULT NULL,
  `sequence` int(11) NOT NULL,
  PRIMARY KEY (`productID`,`productYear`,`paramID`,`paramCategory`),
  KEY `productparameters_category_idx` (`paramCategory`),
  CONSTRAINT `productparameters_category` FOREIGN KEY (`paramCategory`) REFERENCES `productparameterscategories` (`paramcatid`),
  CONSTRAINT `productparameters_ibfk_1` FOREIGN KEY (`productID`, `productYear`) REFERENCES `productionplan` (`id`, `anno`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productparameters`
--

LOCK TABLES `productparameters` WRITE;
/*!40000 ALTER TABLE `productparameters` DISABLE KEYS */;
/*!40000 ALTER TABLE `productparameters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productparameterscategories`
--

DROP TABLE IF EXISTS `productparameterscategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `productparameterscategories` (
  `paramCatID` int(11) NOT NULL,
  `paramCatName` varchar(255) DEFAULT NULL,
  `paramCatDescription` text,
  PRIMARY KEY (`paramCatID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productparameterscategories`
--

LOCK TABLES `productparameterscategories` WRITE;
/*!40000 ALTER TABLE `productparameterscategories` DISABLE KEYS */;
/*!40000 ALTER TABLE `productparameterscategories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `registroeventiproduzione`
--

DROP TABLE IF EXISTS `registroeventiproduzione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `registroeventiproduzione` (
  `TipoEvento` varchar(255) NOT NULL,
  `taskID` int(11) NOT NULL,
  `segnalato` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`TipoEvento`,`taskID`),
  KEY `registroeventi_FK1` (`taskID`),
  CONSTRAINT `registroeventi_FK1` FOREIGN KEY (`taskID`) REFERENCES `tasksproduzione` (`taskid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `registroeventiproduzione`
--

LOCK TABLES `registroeventiproduzione` WRITE;
/*!40000 ALTER TABLE `registroeventiproduzione` DISABLE KEYS */;
/*!40000 ALTER TABLE `registroeventiproduzione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `registroeventitaskproduzione`
--

DROP TABLE IF EXISTS `registroeventitaskproduzione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `registroeventitaskproduzione` (
  `id` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  `task` int(11) NOT NULL,
  `data` datetime NOT NULL,
  `evento` char(1) NOT NULL,
  `note` text,
  PRIMARY KEY (`id`),
  KEY `registroEventiTask_FK1` (`user`),
  KEY `registroEventiTask_FK2` (`task`),
  CONSTRAINT `registroEventiTask_FK1` FOREIGN KEY (`user`) REFERENCES `users` (`userid`),
  CONSTRAINT `registroEventiTask_FK2` FOREIGN KEY (`task`) REFERENCES `tasksproduzione` (`taskid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `registroeventitaskproduzione`
--

LOCK TABLES `registroeventitaskproduzione` WRITE;
/*!40000 ALTER TABLE `registroeventitaskproduzione` DISABLE KEYS */;
/*!40000 ALTER TABLE `registroeventitaskproduzione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `registrooperatoripostazioni`
--

DROP TABLE IF EXISTS `registrooperatoripostazioni`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `registrooperatoripostazioni` (
  `username` varchar(255) NOT NULL,
  `postazione` int(11) NOT NULL,
  `login` datetime NOT NULL,
  `logout` datetime DEFAULT NULL,
  PRIMARY KEY (`username`,`postazione`,`login`),
  KEY `FK_operatorePostazione` (`username`),
  KEY `FK_operatorePostazione_Postazione` (`postazione`),
  CONSTRAINT `FK_operatorePostazione_Postazione` FOREIGN KEY (`postazione`) REFERENCES `postazioni` (`idpostazioni`),
  CONSTRAINT `FK_operatorePostazione_op` FOREIGN KEY (`username`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `registrooperatoripostazioni`
--

LOCK TABLES `registrooperatoripostazioni` WRITE;
/*!40000 ALTER TABLE `registrooperatoripostazioni` DISABLE KEYS */;
/*!40000 ALTER TABLE `registrooperatoripostazioni` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `relazioniprocessi`
--

DROP TABLE IF EXISTS `relazioniprocessi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `relazioniprocessi` (
  `RelazioneID` int(11) NOT NULL DEFAULT '0',
  `Name` text,
  `Description` text,
  `imgUrl` varchar(255) NOT NULL,
  PRIMARY KEY (`RelazioneID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `relazioniprocessi`
--

LOCK TABLES `relazioniprocessi` WRITE;
/*!40000 ALTER TABLE `relazioniprocessi` DISABLE KEYS */;
INSERT INTO `relazioniprocessi` VALUES (0,'#ND','','/img/iconQuestionMark.png'),(1,'Pull','','/img/pull.gif'),(2,'FIFO lane','','/img/fifo_lane.gif'),(3,'Push','','/img/push.gif');
/*!40000 ALTER TABLE `relazioniprocessi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reparti`
--

DROP TABLE IF EXISTS `reparti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `reparti` (
  `idreparto` int(11) NOT NULL,
  `nome` varchar(255) NOT NULL,
  `descrizione` varchar(255) DEFAULT NULL,
  `cadenza` double DEFAULT NULL,
  `splitTasks` bit(1) NOT NULL,
  `anticipoTasks` int(11) NOT NULL DEFAULT '0',
  `ModoCalcoloTC` bit(1) NOT NULL DEFAULT b'0',
  `timezone` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`idreparto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reparti`
--

LOCK TABLES `reparti` WRITE;
/*!40000 ALTER TABLE `reparti` DISABLE KEYS */;
INSERT INTO `reparti` VALUES (0,'Test KaizenKey Department','Test dept',0,'',0,'\0','W. Europe Standard Time');
/*!40000 ALTER TABLE `reparti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `repartipostazioniattivita`
--

DROP TABLE IF EXISTS `repartipostazioniattivita`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `repartipostazioniattivita` (
  `reparto` int(11) NOT NULL,
  `postazione` int(11) NOT NULL,
  `processo` int(11) NOT NULL,
  `revProc` int(11) NOT NULL,
  `variante` int(11) NOT NULL,
  PRIMARY KEY (`reparto`,`postazione`,`processo`,`revProc`,`variante`),
  KEY `postazioniprocessi_FK_1` (`postazione`),
  KEY `postazioniprocessi_FK_2` (`processo`,`revProc`),
  KEY `repartipostazioniattivita_FK_2` (`reparto`),
  KEY `repartipostazioniattivita_FK_3` (`processo`,`revProc`,`variante`),
  CONSTRAINT `postazioniprocessi_FK_1` FOREIGN KEY (`postazione`) REFERENCES `postazioni` (`idpostazioni`),
  CONSTRAINT `repartipostazioniattivita_FK_2` FOREIGN KEY (`reparto`) REFERENCES `reparti` (`idreparto`),
  CONSTRAINT `repartipostazioniattivita_FK_3` FOREIGN KEY (`processo`, `revProc`, `variante`) REFERENCES `processipadrifigli` (`task`, `revtask`, `variante`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `repartipostazioniattivita`
--

LOCK TABLES `repartipostazioniattivita` WRITE;
/*!40000 ALTER TABLE `repartipostazioniattivita` DISABLE KEYS */;
INSERT INTO `repartipostazioniattivita` VALUES (0,20,9,0,5),(0,21,8,0,5),(0,21,14,0,5),(0,22,6,0,5),(0,22,7,0,5);
/*!40000 ALTER TABLE `repartipostazioniattivita` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `repartiprocessi`
--

DROP TABLE IF EXISTS `repartiprocessi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `repartiprocessi` (
  `idReparto` int(11) NOT NULL,
  `processID` int(11) NOT NULL,
  `revisione` int(11) NOT NULL,
  `variante` int(11) NOT NULL,
  PRIMARY KEY (`idReparto`,`variante`,`revisione`,`processID`),
  KEY `repartiprocessi_FK_1` (`processID`,`revisione`,`variante`),
  KEY `repartiprocessi_FK_2` (`idReparto`),
  KEY `repartiprocessi_FK_3` (`variante`),
  CONSTRAINT `repartiprocessi_FK_1` FOREIGN KEY (`processID`, `revisione`, `variante`) REFERENCES `variantiprocessi` (`processo`, `revproc`, `variante`),
  CONSTRAINT `repartiprocessi_FK_2` FOREIGN KEY (`idReparto`) REFERENCES `reparti` (`idreparto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `repartiprocessi`
--

LOCK TABLES `repartiprocessi` WRITE;
/*!40000 ALTER TABLE `repartiprocessi` DISABLE KEYS */;
INSERT INTO `repartiprocessi` VALUES (0,16,0,5);
/*!40000 ALTER TABLE `repartiprocessi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `risorseturnopostazione`
--

DROP TABLE IF EXISTS `risorseturnopostazione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `risorseturnopostazione` (
  `idturno` int(11) NOT NULL,
  `idpostazione` int(11) NOT NULL,
  `risorse` int(11) NOT NULL,
  PRIMARY KEY (`idturno`,`idpostazione`),
  KEY `FK_Turno_res_idx` (`idturno`),
  KEY `FK_postazione_res_idx` (`idpostazione`),
  CONSTRAINT `FK_Turno_res` FOREIGN KEY (`idturno`) REFERENCES `turniproduzione` (`id`),
  CONSTRAINT `FK_postazione_res` FOREIGN KEY (`idpostazione`) REFERENCES `postazioni` (`idpostazioni`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `risorseturnopostazione`
--

LOCK TABLES `risorseturnopostazione` WRITE;
/*!40000 ALTER TABLE `risorseturnopostazione` DISABLE KEYS */;
/*!40000 ALTER TABLE `risorseturnopostazione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `straordinarifestivita`
--

DROP TABLE IF EXISTS `straordinarifestivita`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `straordinarifestivita` (
  `id` int(11) NOT NULL,
  `azione` char(1) NOT NULL COMMENT 'azione: + è straordinario, - è festività',
  `datainizio` datetime NOT NULL,
  `datafine` datetime NOT NULL,
  `turno` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `reparto_FK` (`turno`),
  CONSTRAINT `turno_straord_fest_FK` FOREIGN KEY (`turno`) REFERENCES `turniproduzione` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `straordinarifestivita`
--

LOCK TABLES `straordinarifestivita` WRITE;
/*!40000 ALTER TABLE `straordinarifestivita` DISABLE KEYS */;
/*!40000 ALTER TABLE `straordinarifestivita` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `syslog`
--

DROP TABLE IF EXISTS `syslog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `syslog` (
  `date` datetime NOT NULL,
  `user` varchar(255) NOT NULL,
  `module` varchar(255) NOT NULL,
  `itemtype` varchar(255) NOT NULL,
  `itemid` varchar(45) NOT NULL,
  `notes` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `syslog`
--

LOCK TABLES `syslog` WRITE;
/*!40000 ALTER TABLE `syslog` DISABLE KEYS */;
/*!40000 ALTER TABLE `syslog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `task_microsteps`
--

DROP TABLE IF EXISTS `task_microsteps`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `task_microsteps` (
  `taskid` int(11) NOT NULL,
  `taskrev` int(11) NOT NULL,
  `variantid` int(11) NOT NULL,
  `microstep_id` int(11) NOT NULL,
  `microstep_rev` int(11) NOT NULL,
  `sequence` int(11) NOT NULL,
  `cycletime` int(11) NOT NULL DEFAULT '0',
  `value_or_waste` char(1) NOT NULL DEFAULT 'W' COMMENT 'V = value\\nW = evident waste\\nH = hidden waste',
  PRIMARY KEY (`taskid`,`taskrev`,`variantid`,`microstep_id`,`sequence`,`microstep_rev`),
  KEY `microstep_FK2_idx` (`microstep_id`,`microstep_rev`),
  CONSTRAINT `macrostep_FK1` FOREIGN KEY (`taskid`, `taskrev`, `variantid`) REFERENCES `processipadrifigli` (`task`, `revtask`, `variante`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `task_microsteps`
--

LOCK TABLES `task_microsteps` WRITE;
/*!40000 ALTER TABLE `task_microsteps` DISABLE KEYS */;
INSERT INTO `task_microsteps` VALUES (4,0,2,1,0,0,0,'V');
/*!40000 ALTER TABLE `task_microsteps` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taskparameters`
--

DROP TABLE IF EXISTS `taskparameters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `taskparameters` (
  `TaskID` int(11) NOT NULL,
  `paramID` int(11) NOT NULL,
  `paramCategory` int(11) NOT NULL,
  `paramName` varchar(255) DEFAULT NULL,
  `paramDescription` varchar(255) DEFAULT NULL,
  `isFixed` bit(1) DEFAULT NULL,
  `isRequired` bit(1) DEFAULT NULL,
  `sequence` int(11) DEFAULT NULL,
  `CreatedBy` varchar(255) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`paramID`,`paramCategory`,`TaskID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taskparameters`
--

LOCK TABLES `taskparameters` WRITE;
/*!40000 ALTER TABLE `taskparameters` DISABLE KEYS */;
/*!40000 ALTER TABLE `taskparameters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taskreschedulelog`
--

DROP TABLE IF EXISTS `taskreschedulelog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `taskreschedulelog` (
  `task` int(11) NOT NULL,
  `user` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `rescheduledate` datetime NOT NULL,
  `oldstart` datetime DEFAULT NULL,
  `oldend` datetime DEFAULT NULL,
  `oldworkingtime` datetime DEFAULT NULL,
  `taskreschedulelogcol` varchar(45) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  KEY `taskreschedulelog_user_idx` (`user`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taskreschedulelog`
--

LOCK TABLES `taskreschedulelog` WRITE;
/*!40000 ALTER TABLE `taskreschedulelog` DISABLE KEYS */;
/*!40000 ALTER TABLE `taskreschedulelog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tasksmanuals`
--

DROP TABLE IF EXISTS `tasksmanuals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tasksmanuals` (
  `taskId` int(11) NOT NULL,
  `taskRev` int(11) NOT NULL,
  `taskVarianti` int(11) NOT NULL,
  `manualID` int(11) NOT NULL,
  `manualVersion` int(11) NOT NULL,
  `validityInitialDate` datetime DEFAULT NULL,
  `expiryDate` datetime DEFAULT NULL,
  `sequence` int(11) NOT NULL,
  `isActive` tinyint(4) DEFAULT '1',
  PRIMARY KEY (`taskId`,`taskRev`,`taskVarianti`,`manualID`,`manualVersion`,`sequence`),
  KEY `Variant_FK1_idx` (`taskVarianti`),
  KEY `Manual_FK1_idx` (`manualID`,`manualVersion`),
  CONSTRAINT `Manual_FK1` FOREIGN KEY (`manualID`, `manualVersion`) REFERENCES `manuals` (`id`, `version`),
  CONSTRAINT `Task_FK1` FOREIGN KEY (`taskId`, `taskRev`) REFERENCES `processo` (`processid`, `revisione`),
  CONSTRAINT `Variant_FK1` FOREIGN KEY (`taskVarianti`) REFERENCES `varianti` (`idvariante`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tasksmanuals`
--

LOCK TABLES `tasksmanuals` WRITE;
/*!40000 ALTER TABLE `tasksmanuals` DISABLE KEYS */;
/*!40000 ALTER TABLE `tasksmanuals` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tasksproduzione`
--

DROP TABLE IF EXISTS `tasksproduzione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tasksproduzione` (
  `taskID` int(11) NOT NULL,
  `name` text NOT NULL,
  `description` text,
  `earlyStart` datetime NOT NULL,
  `lateStart` datetime NOT NULL,
  `earlyFinish` datetime NOT NULL,
  `lateFinish` datetime NOT NULL,
  `origTask` int(11) NOT NULL,
  `revOrigTask` int(11) NOT NULL,
  `variante` int(11) NOT NULL,
  `reparto` int(11) NOT NULL,
  `postazione` int(11) NOT NULL,
  `status` varchar(1) NOT NULL,
  `idcommessa` int(11) NOT NULL,
  `annocommessa` year(4) NOT NULL,
  `idArticolo` int(11) NOT NULL,
  `annoArticolo` year(4) NOT NULL,
  `nOperatori` int(11) NOT NULL,
  `tempoCiclo` time NOT NULL,
  `qtaPrevista` int(11) NOT NULL,
  `qtaProdotta` int(11) NOT NULL,
  `endDateReal` datetime DEFAULT NULL,
  `LeadTime` time DEFAULT '00:00:00',
  `WorkingTime` time DEFAULT '00:00:00',
  `Delay` time DEFAULT '00:00:00',
  PRIMARY KEY (`taskID`),
  KEY `tasksproduzione_FK_2` (`postazione`),
  KEY `tasksproduzione_FK_4` (`reparto`),
  KEY `FK_articoliCommesse` (`idcommessa`,`annocommessa`,`idArticolo`,`annoArticolo`),
  KEY `tasksproduzione_FK_proc` (`origTask`,`revOrigTask`),
  CONSTRAINT `FK_articoliCommesse` FOREIGN KEY (`idcommessa`, `annocommessa`, `idArticolo`, `annoArticolo`) REFERENCES `productionplan` (`commessa`, `annocommessa`, `id`, `anno`),
  CONSTRAINT `tasksproduzione_FK_2` FOREIGN KEY (`postazione`) REFERENCES `postazioni` (`idpostazioni`),
  CONSTRAINT `tasksproduzione_FK_4` FOREIGN KEY (`reparto`) REFERENCES `reparti` (`idreparto`),
  CONSTRAINT `tasksproduzione_FK_proc` FOREIGN KEY (`origTask`, `revOrigTask`) REFERENCES `processo` (`processid`, `revisione`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tasksproduzione`
--

LOCK TABLES `tasksproduzione` WRITE;
/*!40000 ALTER TABLE `tasksproduzione` DISABLE KEYS */;
INSERT INTO `tasksproduzione` VALUES (0,'Prelievo accessori','Prelievo accessori (tutti i componeti necessari per l&#39;assemblaggio finale della porta)','2021-04-23 10:00:00','2021-04-27 10:00:00','2021-04-26 10:00:00','2021-04-28 10:00:00',6,0,5,0,22,'N',0,2021,0,2021,1,'04:00:00',4,4,NULL,NULL,NULL,NULL),(1,'Prelievo profili','Prelievo profili in PVC e Alluminio per il taglio','2021-04-23 10:00:00','2021-04-23 10:00:00','2021-04-26 10:00:00','2021-04-26 10:00:00',7,0,5,0,22,'N',0,2021,0,2021,1,'04:00:00',4,4,NULL,NULL,NULL,NULL),(2,'Taglio profili a misura','Taglio profili a misura PVC e Alluminio','2021-04-26 10:00:00','2021-04-26 10:00:00','2021-04-27 10:00:00','2021-04-27 10:00:00',8,0,5,0,21,'N',0,2021,0,2021,1,'04:00:00',4,4,NULL,NULL,NULL,NULL),(3,'Preparazione per imballaggio','Preparazione per imballaggio','2021-04-28 10:00:00','2021-04-28 10:00:00','2021-04-29 10:00:00','2021-04-29 10:00:00',9,0,5,0,21,'N',0,2021,0,2021,1,'04:00:00',4,4,NULL,NULL,NULL,NULL),(4,'Inserimento rinforzo su profili e saldatura','Inserimento rinforzo su profili e saldatura profili','2021-04-27 10:00:00','2021-04-27 10:00:00','2021-04-28 10:00:00','2021-04-28 10:00:00',14,0,5,0,21,'N',0,2021,0,2021,1,'04:00:00',4,4,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `tasksproduzione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tasksproduzioneoperatornotes`
--

DROP TABLE IF EXISTS `tasksproduzioneoperatornotes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tasksproduzioneoperatornotes` (
  `taskID` int(11) NOT NULL,
  `commentID` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `user` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `notes` text CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`taskID`,`commentID`),
  KEY `taskNotes_users_FK2_idx` (`user`),
  CONSTRAINT `taskNotes_taskID_FK1` FOREIGN KEY (`taskID`) REFERENCES `tasksproduzione` (`taskid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tasksproduzioneoperatornotes`
--

LOCK TABLES `tasksproduzioneoperatornotes` WRITE;
/*!40000 ALTER TABLE `tasksproduzioneoperatornotes` DISABLE KEYS */;
INSERT INTO `tasksproduzioneoperatornotes` VALUES (20,0,'2020-11-20 14:25:59','op1','Nota di prova'),(36,0,'2020-09-09 10:09:16','admin','Nota di esempio'),(55,0,'2020-11-04 22:03:41','admin','Tutto bene!');
/*!40000 ALTER TABLE `tasksproduzioneoperatornotes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taskstimespans`
--

DROP TABLE IF EXISTS `taskstimespans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `taskstimespans` (
  `id` int(11) NOT NULL,
  `userid` varchar(255) NOT NULL,
  `taskid` int(11) NOT NULL,
  `starteventid` int(11) NOT NULL,
  `starteventdate` datetime NOT NULL,
  `starteventtype` char(1) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `endeventid` int(11) NOT NULL,
  `endeventdate` datetime NOT NULL,
  `endeventtype` char(1) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `duration_sec` double NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `starteventid_UNIQUE` (`starteventid`),
  UNIQUE KEY `endeventid_UNIQUE` (`endeventid`),
  KEY `user_timespans_fk1_idx` (`userid`),
  KEY `timespans_fktask_idx` (`taskid`),
  KEY `timespans_fkevent1_idx` (`starteventid`),
  KEY `timespans_fkevent2_idx` (`endeventid`),
  CONSTRAINT `timespans_fkevent1` FOREIGN KEY (`starteventid`) REFERENCES `registroeventitaskproduzione` (`id`),
  CONSTRAINT `timespans_fkevent2` FOREIGN KEY (`endeventid`) REFERENCES `registroeventitaskproduzione` (`id`),
  CONSTRAINT `timespans_fktask` FOREIGN KEY (`taskid`) REFERENCES `tasksproduzione` (`taskid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taskstimespans`
--

LOCK TABLES `taskstimespans` WRITE;
/*!40000 ALTER TABLE `taskstimespans` DISABLE KEYS */;
INSERT INTO `taskstimespans` VALUES (0,'admin',0,1,'2020-09-01 01:22:32','I',3,'2020-09-01 01:22:37','F',5),(1,'admin',1,0,'2020-09-01 01:22:31','I',2,'2020-09-01 01:22:34','F',3),(2,'admin',2,4,'2020-09-01 01:23:30','I',5,'2020-09-01 01:24:37','F',67),(3,'admin',3,6,'2020-09-01 01:25:14','I',7,'2020-09-01 01:25:38','F',24),(4,'admin',4,8,'2020-09-01 01:26:07','I',9,'2020-09-01 01:26:40','F',33),(5,'admin',5,11,'2020-09-07 11:03:39','I',13,'2020-09-07 11:03:42','F',3),(6,'admin',6,10,'2020-09-07 11:03:34','I',12,'2020-09-07 11:03:40','F',6),(7,'admin',7,14,'2020-09-07 11:04:10','I',15,'2020-09-07 11:05:59','F',109),(8,'admin',8,16,'2020-09-07 11:06:18','I',17,'2020-09-07 11:06:20','F',2),(9,'admin',9,18,'2020-09-07 11:09:55','I',19,'2020-09-07 11:11:31','F',96),(10,'admin',35,24,'2020-09-09 10:09:45','I',25,'2020-09-09 10:09:57','F',12),(11,'admin',36,20,'2020-09-09 10:06:55','I',21,'2020-09-09 10:08:09','P',74),(12,'admin',36,22,'2020-09-09 10:08:11','I',23,'2020-09-09 10:09:31','F',80),(13,'admin',37,26,'2020-09-09 10:11:32','I',27,'2020-09-09 10:11:44','F',12),(14,'admin',38,28,'2020-09-09 10:13:14','I',29,'2020-09-09 10:15:15','F',121),(15,'admin',39,30,'2020-09-09 10:15:35','I',31,'2020-09-09 10:16:46','F',71),(16,'admin',55,32,'2020-11-04 22:01:04','I',33,'2020-11-04 22:04:03','F',179),(17,'admin',56,36,'2020-11-04 22:04:47','I',37,'2020-11-04 22:04:56','F',9),(18,'admin',57,34,'2020-11-04 22:04:11','I',35,'2020-11-04 22:04:18','F',7),(19,'admin',58,38,'2020-11-04 22:05:03','I',39,'2020-11-04 22:05:33','F',30),(20,'admin',59,40,'2020-11-04 22:06:08','I',41,'2020-11-04 22:08:30','F',142);
/*!40000 ALTER TABLE `taskstimespans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taskuser`
--

DROP TABLE IF EXISTS `taskuser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `taskuser` (
  `taskID` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  `exclusive` bit(1) DEFAULT NULL,
  PRIMARY KEY (`taskID`,`user`),
  KEY `usertask_FK` (`user`),
  CONSTRAINT `taskprodid` FOREIGN KEY (`taskID`) REFERENCES `tasksproduzione` (`taskid`),
  CONSTRAINT `usertask_FK` FOREIGN KEY (`user`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taskuser`
--

LOCK TABLES `taskuser` WRITE;
/*!40000 ALTER TABLE `taskuser` DISABLE KEYS */;
/*!40000 ALTER TABLE `taskuser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taskusermodel`
--

DROP TABLE IF EXISTS `taskusermodel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `taskusermodel` (
  `taskid` int(11) NOT NULL,
  `taskrev` int(11) NOT NULL,
  `variantid` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  `exclusive` bit(1) DEFAULT NULL,
  PRIMARY KEY (`taskid`,`taskrev`,`variantid`,`user`),
  CONSTRAINT `task_FK` FOREIGN KEY (`taskid`, `taskrev`, `variantid`) REFERENCES `processipadrifigli` (`task`, `revtask`, `variante`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taskusermodel`
--

LOCK TABLES `taskusermodel` WRITE;
/*!40000 ALTER TABLE `taskusermodel` DISABLE KEYS */;
/*!40000 ALTER TABLE `taskusermodel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tempiciclo`
--

DROP TABLE IF EXISTS `tempiciclo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tempiciclo` (
  `processo` int(11) NOT NULL,
  `revisione` int(11) NOT NULL,
  `variante` int(11) NOT NULL,
  `num_op` int(11) NOT NULL,
  `setup` time NOT NULL DEFAULT '00:00:00',
  `tempo` time NOT NULL DEFAULT '00:00:00',
  `tunload` time NOT NULL DEFAULT '00:00:00',
  `def` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`processo`,`revisione`,`variante`,`num_op`),
  KEY `FK_processo` (`processo`,`revisione`,`variante`),
  KEY `FK_processovariante` (`processo`,`revisione`,`variante`),
  CONSTRAINT `FK_processovariante` FOREIGN KEY (`processo`, `revisione`, `variante`) REFERENCES `processipadrifigli` (`task`, `revtask`, `variante`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tempiciclo`
--

LOCK TABLES `tempiciclo` WRITE;
/*!40000 ALTER TABLE `tempiciclo` DISABLE KEYS */;
INSERT INTO `tempiciclo` VALUES (4,0,2,1,'00:00:00','01:00:00','00:00:00',''),(5,0,2,1,'00:00:00','01:00:00','00:00:00',''),(6,0,3,1,'00:00:00','01:00:00','00:00:00',''),(6,0,4,1,'00:00:00','01:00:00','00:00:00',''),(6,0,5,1,'00:00:00','01:00:00','00:00:00',''),(6,0,5,2,'01:00:00','01:15:00','00:00:00','\0'),(7,0,4,1,'00:00:00','01:00:00','00:00:00',''),(7,0,5,1,'00:00:00','01:00:00','00:00:00',''),(8,0,3,1,'00:00:00','01:00:00','00:00:00',''),(8,0,4,1,'00:00:00','01:00:00','00:00:00',''),(8,0,5,1,'00:00:00','01:00:00','00:00:00',''),(9,0,3,1,'00:00:00','01:00:00','00:00:00',''),(9,0,4,1,'00:00:00','01:00:00','00:00:00',''),(9,0,5,1,'00:00:00','01:00:00','00:00:00',''),(10,0,3,1,'00:00:00','01:00:00','00:00:00',''),(11,0,3,1,'00:00:00','01:00:00','00:00:00',''),(12,0,3,1,'00:00:00','01:00:00','00:00:00',''),(13,0,3,1,'00:00:00','01:00:00','00:00:00',''),(14,0,3,1,'00:00:00','01:00:00','00:00:00',''),(14,0,5,1,'00:00:00','01:00:00','00:00:00',''),(15,0,3,1,'00:00:00','01:00:00','00:00:00',''),(17,0,6,1,'00:00:00','01:00:00','00:00:00',''),(18,0,6,1,'00:00:00','01:00:00','00:00:00',''),(19,0,6,1,'01:00:00','00:00:00','00:00:00',''),(20,0,6,1,'00:00:00','01:00:00','00:00:00','');
/*!40000 ALTER TABLE `tempiciclo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipipermessi`
--

DROP TABLE IF EXISTS `tipipermessi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tipipermessi` (
  `id` varchar(1) NOT NULL,
  `descrizione` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipipermessi`
--

LOCK TABLES `tipipermessi` WRITE;
/*!40000 ALTER TABLE `tipipermessi` DISABLE KEYS */;
INSERT INTO `tipipermessi` VALUES ('0','Nessun permesso'),('m','Manage'),('r','Read'),('w','Write');
/*!40000 ALTER TABLE `tipipermessi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `turniproduzione`
--

DROP TABLE IF EXISTS `turniproduzione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `turniproduzione` (
  `id` int(11) NOT NULL,
  `reparto` int(11) NOT NULL,
  `nome` varchar(255) NOT NULL DEFAULT '0',
  `colore` varchar(7) DEFAULT '#000000',
  PRIMARY KEY (`id`),
  KEY `turniproduzione_FK_1` (`reparto`),
  CONSTRAINT `turniproduzione_FK_1` FOREIGN KEY (`reparto`) REFERENCES `reparti` (`idreparto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `turniproduzione`
--

LOCK TABLES `turniproduzione` WRITE;
/*!40000 ALTER TABLE `turniproduzione` DISABLE KEYS */;
INSERT INTO `turniproduzione` VALUES (0,0,'Diurno','#0000FF');
/*!40000 ALTER TABLE `turniproduzione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `useremail`
--

DROP TABLE IF EXISTS `useremail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `useremail` (
  `userID` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `forAlarm` bit(1) NOT NULL,
  `Note` varchar(255) NOT NULL,
  PRIMARY KEY (`userID`,`email`),
  KEY `user_FK1` (`userID`),
  CONSTRAINT `user_FK1` FOREIGN KEY (`userID`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useremail`
--

LOCK TABLES `useremail` WRITE;
/*!40000 ALTER TABLE `useremail` DISABLE KEYS */;
INSERT INTO `useremail` VALUES ('produzione','mgriso@kaizenkey.com','','Office'),('spedizioni','mgriso@kaizenkey.com','','Office'),('timeadmin','mgriso@kaizenkey.com','','Casa');
/*!40000 ALTER TABLE `useremail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userphonenumbers`
--

DROP TABLE IF EXISTS `userphonenumbers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `userphonenumbers` (
  `userID` varchar(255) NOT NULL,
  `phoneNumber` varchar(255) NOT NULL,
  `forAlarm` bit(1) NOT NULL,
  `Note` varchar(255) NOT NULL,
  PRIMARY KEY (`userID`,`phoneNumber`),
  KEY `userphone_FK1` (`userID`),
  CONSTRAINT `userphone_FK1` FOREIGN KEY (`userID`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userphonenumbers`
--

LOCK TABLES `userphonenumbers` WRITE;
/*!40000 ALTER TABLE `userphonenumbers` DISABLE KEYS */;
/*!40000 ALTER TABLE `userphonenumbers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `users` (
  `userID` varchar(255) NOT NULL,
  `password` text NOT NULL,
  `nome` text NOT NULL,
  `cognome` text NOT NULL,
  `tipoUtente` varchar(255) NOT NULL,
  `lastlogin` datetime DEFAULT NULL,
  `ID` int(11) NOT NULL,
  `language` varchar(5) NOT NULL DEFAULT 'en',
  `region` varchar(45) DEFAULT NULL,
  `verified` bit(1) NOT NULL,
  `checksum` varchar(45) DEFAULT NULL,
  `creationdate` datetime DEFAULT NULL,
  `destinationURL` varchar(255) DEFAULT NULL,
  `enabled` bit(1) DEFAULT b'1',
  PRIMARY KEY (`userID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES ('admin','f7957eeb5268c62ac063781effb66241','Matteo','Griso','Admin','2021-02-24 10:20:21',0,'it',NULL,'','637492626611331323','2021-02-18 16:31:01',NULL,''),('op1','5eda0686d1a352572ea06a000e5f79c7','Input','Point 1','User','2021-02-18 22:16:46',1,'it',NULL,'','637492676903853610','2021-02-18 17:54:50',NULL,''),('op2','f90a07a0f460574ca7da086f1556066a','Input','Point 2','User','2021-02-18 10:23:45',4,'it',NULL,'','637492838251577546','2021-02-18 22:23:45',NULL,''),('op3','ace514d2880f8a50ce24239225cc357f','Input','Point 3','User','2021-02-21 01:42:43',6,'it',NULL,'','637495117637775061','2021-02-21 13:42:43',NULL,''),('op4','fb7a5a3c2616053e5ea9ac30098ddd5f','Input','Point 4','User','2021-02-21 01:43:16',7,'it',NULL,'','637495117964094203','2021-02-21 13:43:16',NULL,''),('op5','8e688d995f1547a8e289894789b07a49','Input','Point 5','User','2021-02-21 01:43:46',8,'it',NULL,'','637495118265511714','2021-02-21 13:43:46',NULL,''),('op6','71cb11492d755efd5c766a6984db3dd2','Input','Point 6','User','2021-02-21 01:44:08',9,'it',NULL,'','637495118485610545','2021-02-21 13:44:08',NULL,''),('op7','15eacbd7f6d8e8961340f3c57a9b75a0','Input','Point 7','User','2021-02-21 01:44:57',10,'it',NULL,'','637495118976384441','2021-02-21 13:44:57',NULL,''),('op8','a196c372bcef245f26db484fdc7a9cf6','Inout','Point 8','User','2021-02-21 01:45:31',11,'it',NULL,'','637495119314657684','2021-02-21 13:45:31',NULL,''),('op9','df13a8866686efb19d55f249d4f2ec99','Input','Point 9','User','2021-02-21 01:46:12',12,'it',NULL,'','637495119728907832','2021-02-21 13:46:12',NULL,''),('produzione','6d352a66174cb82aa2e6ff2dbd7ce42b','Produzione','Utente','User','2021-02-25 21:43:33',3,'it',NULL,'','637492835899979148','2021-02-18 22:19:50','~/FreeTimeMeasurement/FreeMeasurement/Execute?DepartmentId=1',''),('spedizioni','5ca286974c6eddc33e5eb2e09914e844','Spedizioni','Spedizioni','User','2021-02-24 16:24:49',2,'it',NULL,'','637492677709322513','2021-02-18 17:56:10','~/FreeTimeMeasurement/FreeMeasurement/Execute?DepartmentId=2',''),('timeadmin','5d62c79fcddadce8b52e619fca2a31b9','Coldtech','Coldtech','User','2021-02-25 21:43:55',5,'it',NULL,'','637494371101948869','2021-02-20 16:58:30',NULL,'');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userslog`
--

DROP TABLE IF EXISTS `userslog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `userslog` (
  `time` timestamp(6) NOT NULL,
  `user` varchar(255) NOT NULL,
  `type` varchar(45) NOT NULL,
  `detail` varchar(45) NOT NULL,
  `querystring` varchar(255) NOT NULL DEFAULT '',
  `ip` varchar(45) NOT NULL,
  PRIMARY KEY (`time`,`user`,`ip`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userslog`
--

LOCK TABLES `userslog` WRITE;
/*!40000 ALTER TABLE `userslog` DISABLE KEYS */;
INSERT INTO `userslog` VALUES ('2021-02-18 15:53:25.865000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','red=%2fConfiguration%2fwizConfigLogo.aspx','151.38.211.153'),('2021-02-18 15:53:39.955000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','red=%2fConfiguration%2fwizConfigLogo.aspx','151.38.211.153'),('2021-02-18 16:16:17.725000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','red=%2fConfiguration%2fwizConfigReparti_Main.aspx','151.38.211.153'),('2021-02-18 16:16:26.688000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','red=%2fConfiguration%2fwizConfigReparti_Main.aspx','151.38.211.153'),('2021-02-18 16:18:34.623000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:18:54.240000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:03.260000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:03.751000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:06.933000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:15.141000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:15.768000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:19.100000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:49.714000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:50.386000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:51.104000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:19:57.948000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:20:09.542000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:20:10.214000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:20:12.808000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:20:13.495000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:20:58.479000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:07.667000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:08.308000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:12.183000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:21.198000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:21.698000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:24.136000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:24.698000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:27.714000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:35.089000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:35.808000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:38.073000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:45.745000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:46.479000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:21:49.808000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:22:03.589000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:22:04.308000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:22:09.464000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:22:20.261000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:22:20.698000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:22:23.183000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:22:33.558000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:22:34.214000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=0','151.38.211.153'),('2021-02-18 16:30:51.386000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:09.886000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:24.917000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:25.464000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:29.542000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:37.558000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:38.261000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:40.823000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:47.761000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:48.292000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:50.495000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:58.792000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:31:59.464000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:01.792000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:10.745000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:11.542000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:14.042000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:21.433000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:21.995000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:25.152000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:34.355000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:35.074000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:38.136000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:46.167000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:46.730000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:49.183000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:57.683000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:32:58.339000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:33:03.792000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:33:13.402000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:33:13.902000','admin','Page','/coldtech/Reparti/configOrariTurno.aspx','id=1','151.38.211.153'),('2021-02-18 16:39:55.418000','admin','Page','/coldtech/HomePage/Default.aspx','','151.38.211.153'),('2021-02-18 16:40:00.340000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.38.211.153'),('2021-02-18 16:54:08.729000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.211.153'),('2021-02-18 16:54:13.682000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.211.153'),('2021-02-18 16:54:19.838000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.211.153'),('2021-02-18 16:54:49.807000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.211.153'),('2021-02-18 16:54:51.088000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.211.153'),('2021-02-18 16:55:33.666000','admin','Page','/coldtech/Users/editUser.aspx','id=op1','151.38.211.153'),('2021-02-18 16:55:42.291000','admin','Page','/coldtech/Users/editUser.aspx','id=op1','151.38.211.153'),('2021-02-18 16:55:52.385000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.211.153'),('2021-02-18 16:55:56.901000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.211.153'),('2021-02-18 16:56:10.369000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.211.153'),('2021-02-18 16:56:11.400000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.211.153'),('2021-02-18 16:56:22.963000','admin','Controller','/Users/Users/WorkHoursManualRegistration','','151.38.211.153'),('2021-02-18 16:56:28.588000','admin','Page','/coldtech/Users/editUser.aspx','id=spedizioni','151.38.211.153'),('2021-02-18 16:56:38.854000','admin','Page','/coldtech/Users/editUser.aspx','id=spedizioni','151.38.211.153'),('2021-02-18 16:56:46.276000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.38.211.153'),('2021-02-18 16:58:24.510000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.38.211.153'),('2021-02-18 16:58:39.510000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.38.211.153'),('2021-02-18 16:58:45.322000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.38.211.153'),('2021-02-18 16:58:46.213000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.38.211.153'),('2021-02-18 16:58:49.041000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.38.211.153'),('2021-02-18 16:58:51.057000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=5','151.38.211.153'),('2021-02-18 16:59:00.682000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=5','151.38.211.153'),('2021-02-18 16:59:12.901000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=5','151.38.211.153'),('2021-02-18 16:59:13.651000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=5','151.38.211.153'),('2021-02-18 16:59:19.807000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=5','151.38.211.153'),('2021-02-18 16:59:31.385000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=5','151.38.211.153'),('2021-02-18 16:59:32.213000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=5','151.38.211.153'),('2021-02-18 16:59:44.979000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.211.153'),('2021-02-18 17:00:57.588000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.211.153'),('2021-02-18 17:01:10.713000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.211.153'),('2021-02-18 17:01:11.244000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.211.153'),('2021-02-18 17:01:17.275000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.211.153'),('2021-02-18 17:01:22.384000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.211.153'),('2021-02-18 17:01:22.869000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.211.153'),('2021-02-18 17:01:25.634000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.211.153'),('2021-02-18 17:01:33.088000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.211.153'),('2021-02-18 17:01:33.853000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.211.153'),('2021-02-18 17:01:36.275000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.211.153'),('2021-02-18 17:01:42.291000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.211.153'),('2021-02-18 17:01:42.712000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.211.153'),('2021-02-18 17:10:21.119000','admin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.38.211.153'),('2021-02-18 17:10:30.697000','admin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.38.211.153'),('2021-02-18 17:10:55.806000','admin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.38.211.153'),('2021-02-18 17:11:12.056000','admin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.38.211.153'),('2021-02-18 17:11:17.728000','admin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.38.211.153'),('2021-02-18 17:12:27.916000','admin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.38.211.153'),('2021-02-18 17:13:49.556000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.211.153'),('2021-02-18 17:13:54.462000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3','151.38.211.153'),('2021-02-18 17:14:21.978000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3','151.38.211.153'),('2021-02-18 17:14:23.681000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:14:25.353000','admin','Controller','/Products/ProductParameters/Index','processID=3&processRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:14:31.275000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:14:31.978000','admin','Controller','/Products/ProductParameters/Index','processID=3&processRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:14:43.134000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:14:55.025000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:14:56.588000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:14:58.494000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:15:00.088000','admin','Controller','/Products/ProductParameters/Index','processID=3&processRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:15:03.713000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:15:04.728000','admin','Controller','/Products/ProductParameters/Index','processID=3&processRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:15:09.463000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=4&act=updatepos&posx=138&posy=160&variante=2','151.38.211.153'),('2021-02-18 17:15:09.884000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=4&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:11.275000','admin','Controller','/Products/Products/TaskParametersList','TaskID=4&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:12.291000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=4&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:13.322000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=4&TaskRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:15:14.166000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=4&TaskRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:15:15.947000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:15:16.400000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=4&TaskRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:15:19.088000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=4&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:21.291000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=4&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:27.541000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:15:28.338000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=4&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:28.744000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=4&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:34.681000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=5&act=updatepos&posx=307&posy=274&variante=2','151.38.211.153'),('2021-02-18 17:15:35.181000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=5&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:35.728000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=5&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:36.759000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=5&TaskRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:15:37.791000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=5&TaskRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:15:38.431000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=5&TaskRev=0&variantID=2','151.38.211.153'),('2021-02-18 17:15:40.369000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:15:40.884000','admin','Controller','/Products/Products/TaskParametersList','TaskID=5&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:43.197000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=5&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:49.369000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=5&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:49.838000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=5&TaskRev=0&VariantID=2','151.38.211.153'),('2021-02-18 17:15:52.025000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:15:53.166000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=4&revTaskID=0&variante=2','151.38.211.153'),('2021-02-18 17:15:56.197000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=4&revTaskID=0&variante=2','151.38.211.153'),('2021-02-18 17:16:01.978000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=5&act=updatepos&posx=417&posy=215&variante=2','151.38.211.153'),('2021-02-18 17:16:03.697000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3&variante=2','151.38.211.153'),('2021-02-18 17:16:13.900000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=3&variante=2&repID=2','151.38.211.153'),('2021-02-18 17:16:17.384000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=3&variante=2&repID=2','151.38.211.153'),('2021-02-18 17:16:18.541000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=3&variante=2&repID=2','151.38.211.153'),('2021-02-18 17:16:21.869000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=3&variante=2&repID=2','151.38.211.153'),('2021-02-18 17:16:22.916000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=3&variante=2&repID=2','151.38.211.153'),('2021-02-18 17:18:19.198000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.211.153'),('2021-02-18 19:05:30.901000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1','151.18.26.52'),('2021-02-18 19:05:41.058000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1','151.18.26.52'),('2021-02-18 19:05:42.573000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:05:43.776000','admin','Controller','/Products/ProductParameters/Index','processID=1&processRev=0&variantID=3','151.18.26.52'),('2021-02-18 19:05:46.948000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:05:47.636000','admin','Controller','/Products/ProductParameters/Index','processID=1&processRev=0&variantID=3','151.18.26.52'),('2021-02-18 19:05:57.730000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:05:59.308000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:06:01.386000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:06:01.792000','admin','Controller','/Products/ProductParameters/Index','processID=1&processRev=0&variantID=3','151.18.26.52'),('2021-02-18 19:06:05.620000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=176&posy=219&variante=3','151.18.26.52'),('2021-02-18 19:06:06.074000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=6&TaskRev=0&VariantID=3','151.18.26.52'),('2021-02-18 19:06:06.542000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=6&TaskRev=0&VariantID=3','151.18.26.52'),('2021-02-18 19:06:07.558000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=6&TaskRev=0&variantID=3','151.18.26.52'),('2021-02-18 19:06:08.574000','admin','Controller','/Products/Products/TaskParametersList','TaskID=6&TaskRev=0&VariantID=3','151.18.26.52'),('2021-02-18 19:06:09.714000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=6&TaskRev=0&variantID=3','151.18.26.52'),('2021-02-18 19:06:10.386000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=6&TaskRev=0&variantID=3','151.18.26.52'),('2021-02-18 19:06:12.527000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:06:14.183000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=6&TaskRev=0&VariantID=3','151.18.26.52'),('2021-02-18 19:06:18.980000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=6&TaskRev=0&VariantID=3','151.18.26.52'),('2021-02-18 19:06:19.433000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=6&TaskRev=0&VariantID=3','151.18.26.52'),('2021-02-18 19:06:24.542000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:06:37.355000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:06:49.355000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:07:01.402000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:07:13.293000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:07:26.496000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:07:38.324000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:07:50.403000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:08:02.465000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:08:14.372000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:08:26.247000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:08:38.263000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:08:50.325000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:09:02.310000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:09:14.482000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:09:26.404000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:09:38.466000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:09:51.279000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:10:03.435000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:10:15.326000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:10:27.326000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:10:39.482000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:10:51.560000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:11:04.342000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:11:16.358000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:11:28.311000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:11:40.436000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:11:52.311000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:12:04.342000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:12:16.374000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:12:28.342000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:12:40.405000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:12:53.327000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:13:05.249000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:13:17.343000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:13:30.249000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:13:42.280000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:13:54.312000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:14:06.312000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:14:18.359000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:14:30.312000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:14:42.296000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:14:54.343000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:15:06.297000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:15:19.328000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:15:31.406000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:15:43.406000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:15:56.328000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:16:08.359000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:16:20.344000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:16:32.297000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:16:44.313000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:16:56.375000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:17:08.422000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:17:20.328000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:17:32.391000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:17:44.375000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:17:56.297000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:18:08.313000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:18:20.313000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:18:32.313000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:18:44.359000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:18:56.297000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:19:08.406000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:19:20.359000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:19:32.266000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:19:44.313000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:19:56.391000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:20:08.313000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:20:20.266000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:20:32.406000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:20:45.297000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:20:57.313000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:21:09.328000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:21:21.360000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:21:33.282000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:21:45.250000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:21:57.297000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:22:09.344000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:22:21.438000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:22:33.423000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:22:45.266000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:22:57.863000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:23:10.360000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:23:22.454000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:23:34.298000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:23:46.345000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:23:59.314000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:24:11.501000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:24:23.470000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:24:36.267000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:24:48.345000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:25:00.298000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:25:12.439000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:25:24.423000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:25:36.236000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:25:48.314000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:26:00.423000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:26:12.486000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:26:24.502000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:26:36.345000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:26:48.299000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:27:01.283000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:27:13.596000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:27:26.361000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:27:38.361000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:27:50.283000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:28:02.361000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:28:14.283000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:28:26.361000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:28:38.346000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:28:50.283000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:29:02.330000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:29:14.346000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:29:26.315000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:29:38.268000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:29:50.315000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:30:02.487000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:30:15.362000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:30:27.284000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:30:39.331000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:30:51.393000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:31:03.393000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:31:15.268000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:31:27.284000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:31:39.346000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:31:51.268000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:32:03.331000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:32:15.347000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:32:27.425000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:32:39.284000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:32:51.315000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:33:03.409000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:33:16.331000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:33:28.316000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:33:40.315000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:33:52.394000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:34:04.300000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:34:16.347000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:34:28.331000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:34:40.331000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:34:52.597000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:35:05.366000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:35:17.268000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:35:29.315000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:35:41.487000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:35:53.440000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:36:05.409000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:36:17.487000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:36:29.393000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:36:41.315000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:36:53.518000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:37:05.300000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:37:17.300000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:37:29.300000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:37:41.315000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:37:53.925000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:38:06.331000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:38:18.395000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:38:30.363000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:38:42.300000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:38:54.472000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:39:06.425000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:39:18.331000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:39:30.316000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:39:42.316000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:39:54.441000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:40:06.457000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:40:19.425000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:40:31.644000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:40:44.379000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:40:56.269000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:41:08.269000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:41:20.332000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:41:32.332000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:41:44.504000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:41:56.363000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:42:08.348000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:42:20.316000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:42:32.301000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:42:44.426000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:42:56.457000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:43:09.363000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:43:21.348000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:43:33.426000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:43:45.426000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:43:57.473000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:44:09.317000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:44:21.364000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:44:33.442000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:44:45.489000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:44:58.520000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:45:10.473000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:45:22.458000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:45:34.442000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:45:47.317000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:45:59.301000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:46:11.348000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:46:23.380000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:46:35.333000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:46:47.348000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:46:59.349000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:47:11.317000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:47:23.474000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:47:35.317000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:47:47.317000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:47:59.458000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:48:12.474000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:48:24.521000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:48:36.255000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:48:48.396000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:49:00.427000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:49:12.177000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:49:24.443000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:49:36.411000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:49:49.255000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:50:02.412000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:50:14.427000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:50:26.412000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:50:39.365000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:50:51.427000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:51:03.412000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:51:16.287000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:51:28.302000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:51:40.286000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:51:52.302000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:52:04.443000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:52:16.287000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:52:28.005000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:52:39.833000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:52:52.802000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:53:05.474000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:53:18.302000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:53:30.443000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:53:42.849000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:53:55.286000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:54:07.365000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:54:19.349000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:54:31.380000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:54:43.302000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:54:55.255000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:55:07.318000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:55:19.505000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:55:31.381000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:55:43.318000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:55:55.302000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:56:08.303000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:56:21.428000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:56:33.396000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:56:45.381000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:56:57.271000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:57:09.365000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:57:21.365000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:57:33.350000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:57:45.443000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:57:58.350000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:58:10.319000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:58:22.303000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:58:34.459000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:58:46.412000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:58:59.272000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:59:11.319000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:59:23.319000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:59:36.569000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 19:59:49.288000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:00:02.053000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:00:14.553000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:00:27.335000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:00:39.319000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:00:51.303000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:01:04.522000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:01:17.491000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:01:30.366000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:01:43.350000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:01:55.413000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:02:07.585000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:02:20.319000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:02:32.429000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:02:45.444000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:02:57.304000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:03:09.382000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:03:21.382000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:03:33.444000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:03:45.351000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:03:58.476000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:04:11.351000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:04:23.507000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:04:35.507000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:04:47.492000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:04:59.476000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:05:11.507000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:05:24.398000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:05:36.383000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:05:49.367000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:06:01.430000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:06:13.445000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:06:25.476000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:06:37.273000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:06:49.305000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:07:01.398000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:07:13.367000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:07:26.476000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:07:39.367000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:07:51.383000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:08:03.383000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:08:16.383000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:08:28.289000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:08:40.304000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:08:52.304000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:09:04.336000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:09:16.554000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:09:28.320000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:09:40.398000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:09:52.336000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:10:05.492000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:10:18.661000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:10:31.820000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:10:44.485000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:10:56.631000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:11:09.449000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:11:21.638000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:11:35.198000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:11:47.634000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:12:00.503000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:12:12.530000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:12:24.565000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:12:36.499000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:12:48.370000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:13:00.518000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:13:12.505000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:13:24.381000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:13:36.427000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:13:48.542000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:14:00.538000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:14:12.678000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:14:25.756000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:14:38.483000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:14:50.675000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:15:03.553000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:15:15.802000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:15:28.687000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:15:41.424000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:15:54.076000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:16:06.660000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:16:19.508000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:16:31.701000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:16:44.496000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:16:56.553000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:17:08.436000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:17:21.097000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:17:33.688000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:17:46.500000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:17:58.583000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:18:10.495000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:18:23.429000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:18:35.876000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:18:49.123000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:19:02.012000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:19:14.711000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:19:27.769000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:19:41.199000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:19:54.068000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:20:06.692000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:20:20.557000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:20:33.944000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:20:46.762000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:20:59.874000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:21:12.647000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:21:25.678000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:21:38.687000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:21:51.777000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:22:04.669000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:22:17.482000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:22:29.783000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:22:43.344000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:22:55.538000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:23:08.585000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:23:21.534000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:23:33.485000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:23:45.386000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:23:57.415000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:24:09.393000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:24:22.720000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:24:38.060000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:24:50.556000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:25:02.843000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:25:15.731000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:25:28.513000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:25:41.713000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:25:54.500000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:26:06.474000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:26:18.475000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:26:30.597000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:26:43.771000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:26:56.507000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:27:08.585000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:27:20.603000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:27:33.493000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:27:45.549000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:27:57.971000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:28:10.357000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:28:22.570000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:28:34.531000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:28:46.455000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:28:58.637000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:29:10.649000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:29:23.809000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:29:36.676000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:29:50.153000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:30:02.818000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:30:15.662000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:30:28.848000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:30:42.245000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:30:54.778000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:31:07.979000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:31:20.580000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:31:32.675000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:31:40.184000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:31:44.349000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:32:40.781000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:32:44.052000','op1','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:32:46.082000','op1','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:32:46.492000','op1','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:32:47.819000','op1','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 20:32:49.314000','op1','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 20:32:51.431000','op1','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:32:55.243000','op1','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 20:33:02.520000','op1','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:33:15.608000','op1','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:33:24.642000','op1','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:33:35.529000','op1','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:33:46.576000','op1','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:33:47.020000','op1','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:33:51.149000','op1','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:33:51.277000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:33:55.047000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:33:56.447000','admin','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 20:33:58.446000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:34:00.501000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 20:34:09.219000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.18.26.52'),('2021-02-18 20:34:15.632000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:34:21.677000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.18.26.52'),('2021-02-18 20:34:28.584000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:34:36.080000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:34:40.805000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:34:46.635000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:34:53.488000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:34:54.291000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:34:58.688000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:35:05.421000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:35:06.124000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:35:17.512000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:35:19.618000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.18.26.52'),('2021-02-18 20:35:21.645000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:35:29.814000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:35:30.483000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:35:42.645000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:35:56.448000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:36:09.543000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:36:22.689000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:36:35.512000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:36:48.614000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:37:01.624000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:37:14.743000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:37:27.460000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:37:39.660000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:37:54.260000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:38:06.553000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:38:18.514000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:38:30.668000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:38:43.905000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:38:57.698000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:39:10.566000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:39:23.516000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:39:39.696000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:39:56.246000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:40:08.738000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:40:21.797000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:40:34.705000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:40:47.669000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:41:00.433000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:41:12.570000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:41:25.500000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:41:38.515000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:41:50.556000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:42:02.594000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:42:15.000000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:42:27.669000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:42:40.744000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:42:53.905000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:43:06.605000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:43:41.410000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:43:41.490000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:43:41.822000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:43:45.886000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:43:50.441000','admin','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 20:43:52.484000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:43:55.794000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 20:44:05.362000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.18.26.52'),('2021-02-18 20:44:13.692000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.18.26.52'),('2021-02-18 20:44:22.254000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.18.26.52'),('2021-02-18 20:44:23.685000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:44:29.882000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:44:33.478000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:44:37.053000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:44:59.905000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:45:11.229000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.18.26.52'),('2021-02-18 20:45:13.095000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:45:18.582000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:49:21.592000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:49:25.518000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:49:30.200000','admin','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 20:49:34.746000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 20:49:46.239000','admin','Page','/coldtech/Users/managePermessi.aspx','','151.18.26.52'),('2021-02-18 20:49:51.590000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.18.26.52'),('2021-02-18 20:49:58.093000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.18.26.52'),('2021-02-18 20:49:59.945000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:50:05.777000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:50:09.154000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=5','151.18.26.52'),('2021-02-18 20:50:30.774000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.18.26.52'),('2021-02-18 20:50:48.790000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=10','151.18.26.52'),('2021-02-18 20:51:07.402000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=10','151.18.26.52'),('2021-02-18 20:51:20.351000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=10','151.18.26.52'),('2021-02-18 20:51:21.165000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=10','151.18.26.52'),('2021-02-18 20:51:27.027000','admin','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:51:28.721000','admin','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:51:28.848000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:51:33.172000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 20:51:34.467000','op1','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 20:51:35.666000','op1','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 21:13:10.983000','op1','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:13:13.052000','op1','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:13:13.172000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:13:18.762000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:13:20.199000','admin','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 21:13:21.181000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 21:15:36.901000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:15:43.160000','admin','Page','/coldtech/Users/editUser.aspx','id=op1','151.18.26.52'),('2021-02-18 21:15:59.771000','admin','Page','/coldtech/Users/editUser.aspx','id=op1','151.18.26.52'),('2021-02-18 21:16:03.559000','admin','Page','/coldtech/Users/editUser.aspx','id=op1','151.18.26.52'),('2021-02-18 21:16:06.411000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:16:11.619000','admin','Page','/coldtech/Users/editUser.aspx','id=spedizioni','151.18.26.52'),('2021-02-18 21:16:24.547000','admin','Page','/coldtech/Users/editUser.aspx','id=spedizioni','151.18.26.52'),('2021-02-18 21:16:28.379000','admin','Page','/coldtech/Users/editUser.aspx','id=spedizioni','151.18.26.52'),('2021-02-18 21:16:41.046000','admin','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:16:42.767000','admin','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:16:42.869000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:16:46.719000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:16:48.061000','op1','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 21:16:48.842000','op1','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 21:17:03.473000','op1','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:17:04.999000','op1','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:17:05.108000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:17:07.930000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:17:09.241000','admin','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 21:17:11.078000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 21:17:17.421000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:18:46.070000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:19:49.274000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:19:50.103000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:19:56.489000','admin','Page','/coldtech/Users/editUser.aspx','id=produzione','151.18.26.52'),('2021-02-18 21:20:14.548000','admin','Page','/coldtech/Users/editUser.aspx','id=produzione','151.18.26.52'),('2021-02-18 21:20:19.598000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:20:22.345000','admin','Page','/coldtech/Users/editUser.aspx','id=op1','151.18.26.52'),('2021-02-18 21:20:37.166000','admin','Page','/coldtech/Users/editUser.aspx','id=op1','151.18.26.52'),('2021-02-18 21:20:40.635000','admin','Page','/coldtech/Users/editUser.aspx','id=op1','151.18.26.52'),('2021-02-18 21:20:46.240000','admin','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:20:48.035000','admin','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:20:48.162000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:20:52.397000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:20:54.008000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 21:20:55.058000','produzione','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 21:22:47.332000','qgboe2xi0spfhus2jjswx5eo','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 21:22:48.852000','qgboe2xi0spfhus2jjswx5eo','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 21:22:49.561000','qgboe2xi0spfhus2jjswx5eo','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:23:01.717000','qgboe2xi0spfhus2jjswx5eo','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:23:03.633000','admin','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 21:23:04.353000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-18 21:23:12.398000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:23:27.711000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:23:44.433000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:23:45.287000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.18.26.52'),('2021-02-18 21:23:50.049000','admin','Page','/coldtech/Users/editUser.aspx','id=2','151.18.26.52'),('2021-02-18 21:24:34.813000','admin','Page','/coldtech/Users/editUser.aspx','id=2','151.18.26.52'),('2021-02-18 21:24:40.532000','admin','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:24:42.210000','admin','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:24:42.332000','qgboe2xi0spfhus2jjswx5eo','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:24:48.590000','qgboe2xi0spfhus2jjswx5eo','Page','/coldtech/Login/login.aspx','','151.18.26.52'),('2021-02-18 21:25:20.132000','admin','Page','/coldtech/Users/editUser.aspx','id=op2','151.18.26.52'),('2021-02-18 21:25:29.618000','admin','Page','/coldtech/Users/editUser.aspx','id=op2','151.18.26.52'),('2021-02-18 21:25:44.612000','admin','Page','/coldtech/HomePage/Default.aspx','','151.18.26.52'),('2021-02-18 21:25:46.087000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=20/01/2021&endPeriod=20/02/2021&periodType=1','151.18.26.52'),('2021-02-19 06:35:10.265000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/HomePage/Default.aspx','','151.34.54.195'),('2021-02-19 06:35:31.671000','0x23sbjw3xnoa4woqwuc324k','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.34.54.195'),('2021-02-19 06:37:21.734000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.54.195'),('2021-02-19 06:37:33.905000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.54.195'),('2021-02-19 06:37:36.421000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.34.54.195'),('2021-02-19 06:37:37.702000','produzione','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.34.54.195'),('2021-02-19 06:58:43.951000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/HomePage/Default.aspx','','151.34.54.195'),('2021-02-19 06:58:45.522000','4gxgvqbq5f2ih3mjzuvbfr5j','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.34.54.195'),('2021-02-19 06:58:46.687000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Login/login.aspx','','151.34.54.195'),('2021-02-19 06:58:51.483000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Login/login.aspx','','151.34.54.195'),('2021-02-19 06:58:53.627000','admin','Page','/coldtech/HomePage/Default.aspx','','151.34.54.195'),('2021-02-19 06:58:55.923000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.34.54.195'),('2021-02-19 06:59:00.219000','admin','Page','/coldtech/Reparti/listReparti.aspx','','151.34.54.195'),('2021-02-19 06:59:03.830000','admin','Page','/coldtech/Reparti/configReparto.aspx','id=2','151.34.54.195'),('2021-02-19 06:59:09.836000','admin','Controller','/Config/AndonConfig/DepartmentScrollTypeView','','151.34.54.195'),('2021-02-19 06:59:14.857000','admin','Page','/coldtech/Reparti/configReparto.aspx','id=2','151.34.54.195'),('2021-02-19 06:59:22.267000','admin','Page','/coldtech/Reparti/configReparto.aspx','id=2','151.34.54.195'),('2021-02-19 07:20:17.863000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.54.195'),('2021-02-19 07:20:28.236000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.54.195'),('2021-02-19 07:20:36.076000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=11','151.34.54.195'),('2021-02-19 07:20:45.575000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=11','151.34.54.195'),('2021-02-19 07:20:49.986000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=11','151.34.54.195'),('2021-02-19 07:21:01.216000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=11','151.34.54.195'),('2021-02-19 08:06:02.254000','produzione','Page','/coldtech/Login/login.aspx','','151.34.54.195'),('2021-02-19 08:11:43.466000','produzione','Page','/coldtech/Login/login.aspx','','151.34.54.195'),('2021-02-19 10:06:32.412000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.82.34.13'),('2021-02-19 10:07:37.213000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1','151.82.34.13'),('2021-02-19 10:07:41.755000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:07:43.173000','admin','Controller','/Products/ProductParameters/Index','processID=1&processRev=0&variantID=3','151.82.34.13'),('2021-02-19 10:07:54.851000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:08:07.230000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:08:19.316000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:08:31.088000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:08:43.013000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:08:55.214000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:09:07.277000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:09:19.072000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:09:31.049000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:09:43.007000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:09:55.020000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:10:06.925000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:10:18.913000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:10:30.946000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:10:42.863000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:10:54.997000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:11:07.067000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:11:19.137000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:11:30.988000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:11:43.236000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:11:55.068000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:12:06.924000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:12:18.803000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:12:31.049000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:12:43.421000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:12:55.467000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:13:07.698000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:13:19.582000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:13:31.550000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:13:43.387000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:13:55.143000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:14:07.132000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:14:19.193000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:14:31.123000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:14:43.326000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:14:55.235000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:15:07.342000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:15:19.227000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:15:31.538000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:15:43.508000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:15:55.560000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:16:07.555000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:16:19.558000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:16:31.520000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:16:43.411000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:16:55.520000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:17:07.637000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:17:19.483000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:17:32.025000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:17:43.899000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:17:55.937000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:18:08.049000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:18:19.953000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:18:31.904000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:18:43.834000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:18:55.689000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:19:07.675000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:19:19.527000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:19:31.483000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:19:43.312000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:19:55.260000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:20:07.095000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:20:19.007000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:20:31.014000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:20:43.045000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:20:54.901000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:21:06.631000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:21:19.010000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:21:30.973000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:21:42.791000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:21:54.519000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:22:06.274000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:22:18.238000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:22:30.072000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:22:42.089000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:22:54.091000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:23:05.990000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:23:18.274000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:23:30.061000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:23:42.077000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:23:54.075000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:24:05.975000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:24:17.763000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:24:29.703000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:24:41.664000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:24:53.528000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:25:05.626000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:25:17.719000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:25:29.701000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:25:41.700000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:25:53.437000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:26:05.404000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:26:17.386000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:26:29.378000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:26:41.701000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:26:53.602000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:27:05.465000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:27:17.403000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:27:29.318000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:27:41.295000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:27:53.418000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:28:05.497000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:28:17.441000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:28:29.470000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:28:41.325000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:28:53.147000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:29:04.968000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:29:16.958000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:29:28.774000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:29:40.508000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:29:52.393000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:30:04.442000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:30:16.466000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:30:28.372000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:30:40.324000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:30:52.237000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:31:04.129000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:31:16.249000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:31:28.176000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:31:40.226000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:31:51.936000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:32:04.136000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:32:16.061000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:32:28.187000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:32:40.138000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:32:51.925000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:33:03.985000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:33:16.019000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:33:28.025000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:33:40.131000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:33:52.099000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:34:03.814000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:34:15.590000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:34:27.400000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:34:39.083000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:34:50.982000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:35:03.135000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:35:14.959000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:35:26.822000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:35:38.688000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:35:50.492000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:36:02.505000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:36:14.427000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:36:26.282000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:36:38.069000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:36:49.864000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:37:02.009000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:37:13.837000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:37:25.479000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:37:37.340000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:37:49.297000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:38:01.832000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:38:13.772000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:38:25.732000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:38:37.606000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:38:49.317000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:39:01.503000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:39:13.489000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:39:25.427000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:39:37.675000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:39:49.476000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:40:02.590000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:40:14.800000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:40:26.558000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:40:38.393000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:40:50.799000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:41:02.607000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:41:14.446000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:41:26.598000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:41:38.639000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:41:50.403000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:42:03.001000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:42:14.726000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:42:26.675000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:42:38.975000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:42:50.969000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:43:02.982000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:43:15.217000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:43:26.990000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:43:38.921000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:43:51.850000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:44:04.059000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:44:15.920000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:44:28.219000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:44:40.043000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:44:51.838000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:45:04.337000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:45:16.214000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:45:28.296000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:45:54.307000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:46:07.614000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:46:20.057000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:46:32.076000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:46:44.330000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:46:57.171000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:47:10.019000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:47:21.981000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:47:34.103000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:47:46.235000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:47:58.027000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:48:10.147000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:48:22.662000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:48:34.852000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:48:46.901000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:48:59.406000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:49:11.481000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:49:23.855000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:49:36.088000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:49:48.623000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:50:00.832000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:50:13.305000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:50:25.637000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:50:38.739000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:50:51.011000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:51:03.053000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:51:15.334000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:51:27.426000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:51:39.561000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:51:52.084000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:52:04.419000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:52:17.660000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:52:30.085000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:52:43.159000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:52:55.416000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:53:07.822000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:53:20.298000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:53:32.707000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:53:44.797000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:53:58.222000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:54:10.513000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:54:22.825000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:54:35.221000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:54:47.204000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:55:00.078000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:55:12.314000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:55:25.579000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:55:38.686000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:55:51.683000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:56:04.125000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:56:16.578000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:56:29.013000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:56:41.170000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:56:53.111000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:57:05.689000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:57:17.797000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:57:29.793000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:57:42.236000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:57:54.229000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:58:06.300000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:58:29.257000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:58:41.639000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:58:53.346000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:59:05.243000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:59:17.606000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:59:29.701000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:59:41.826000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 10:59:54.141000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:00:06.754000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:00:18.562000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:00:30.782000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:00:42.498000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:00:54.390000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:01:06.744000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:01:18.678000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:01:30.420000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:01:42.729000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:01:54.425000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:02:06.394000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:02:18.659000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:02:30.525000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:02:42.497000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:02:54.788000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:03:06.733000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:03:18.528000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:03:30.741000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:03:42.695000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:03:54.463000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:04:06.705000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:04:18.662000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:04:30.339000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:04:42.725000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:04:54.619000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:05:06.628000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:05:18.893000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:05:30.771000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:05:42.671000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:05:55.008000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:06:06.827000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:06:18.636000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:06:31.064000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:06:43.079000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:06:54.952000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:07:07.405000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:07:19.295000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:07:31.244000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:07:43.868000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:07:56.021000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:08:08.007000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:08:20.885000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:08:33.072000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:08:44.879000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:08:57.125000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:09:08.859000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:09:20.751000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:09:33.260000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:09:45.296000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:09:57.112000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:10:09.437000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:10:21.489000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:10:33.320000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:10:45.659000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:10:57.553000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:11:09.298000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:11:21.859000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:11:33.942000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:11:45.873000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:11:58.357000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:12:10.231000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:12:22.134000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:12:34.330000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:12:46.124000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:12:57.901000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:13:10.820000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:13:22.566000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:13:34.318000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:13:46.661000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:13:58.483000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:14:10.430000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:14:23.020000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:14:34.681000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:14:46.448000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:14:58.914000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:15:10.936000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:15:22.929000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:15:35.214000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:15:47.677000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:15:59.450000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:16:11.797000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:16:23.612000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:16:35.401000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:16:47.931000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:16:59.664000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:17:11.543000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:17:24.030000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:17:36.037000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:17:48.184000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:18:00.530000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:18:12.760000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:18:24.761000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:18:37.195000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:18:49.136000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:18:50.098000','zn4rm2q1axyzkg3p4ugmnlyp','Page','/coldtech/HomePage/Default.aspx','','151.82.34.13'),('2021-02-19 11:18:54.737000','zn4rm2q1axyzkg3p4ugmnlyp','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.82.34.13'),('2021-02-19 11:19:00.586000','zn4rm2q1axyzkg3p4ugmnlyp','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 11:19:01.133000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:19:13.803000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:19:25.736000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:19:38.361000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:19:51.473000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:20:03.429000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:20:04.251000','zn4rm2q1axyzkg3p4ugmnlyp','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 11:20:06.269000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.82.34.13'),('2021-02-19 11:20:08.596000','produzione','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.82.34.13'),('2021-02-19 11:20:15.543000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:20:27.781000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:20:39.768000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:20:51.629000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:21:03.929000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:21:15.733000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:21:27.800000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:21:40.099000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:21:52.125000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:22:03.943000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:22:15.842000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:22:28.199000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:22:40.093000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:22:52.026000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:23:04.441000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:23:16.531000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:23:28.658000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:23:40.995000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:23:52.943000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:24:04.774000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:24:17.219000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:24:29.344000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:24:41.170000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:24:53.450000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:25:05.695000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:25:17.723000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:25:30.089000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:25:41.790000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:25:53.620000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:26:05.797000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:26:17.630000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:26:29.708000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:26:42.229000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:26:54.272000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:27:06.146000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:27:18.587000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:27:30.376000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:27:42.229000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:27:54.731000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:28:06.662000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:28:18.650000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:28:31.403000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:28:43.359000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:28:55.341000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:29:07.697000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:29:19.601000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:29:31.585000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:29:44.092000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:29:56.157000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:30:08.074000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:30:20.529000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:30:32.767000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:30:45.053000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:30:57.595000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:31:09.614000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:31:21.760000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:31:34.132000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:31:46.372000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:31:58.438000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:32:10.816000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:32:23.189000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:32:35.359000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:32:47.908000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:33:00.206000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:33:12.402000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:33:24.772000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:33:36.883000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:33:48.902000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:34:01.320000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:34:13.373000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:34:25.378000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:34:37.819000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:34:49.627000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:35:01.776000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:35:14.106000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:35:26.172000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:35:38.185000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:35:50.544000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:36:02.921000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:36:14.839000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:36:27.157000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:36:39.114000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:36:50.979000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:37:03.441000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:37:15.358000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:37:27.413000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:37:39.809000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:37:51.633000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:38:03.621000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:38:15.513000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:38:27.833000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:38:39.957000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:38:51.947000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:39:04.331000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:39:16.365000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:39:28.368000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:39:40.614000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:39:52.756000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:40:04.813000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:40:17.105000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:40:29.012000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:40:41.120000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:40:53.432000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:41:05.604000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:41:17.737000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:41:30.031000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:41:42.169000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:41:54.228000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:42:06.353000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:42:18.159000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:42:29.935000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:42:42.593000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:42:55.066000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:43:07.221000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:43:19.387000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:43:31.408000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:43:43.398000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:43:55.611000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:44:07.770000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:44:19.752000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:44:32.005000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:44:44.063000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:44:55.945000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:45:08.454000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:45:20.547000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:45:32.613000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:45:44.835000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:45:57.069000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:46:09.030000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:46:21.562000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:46:33.960000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:46:45.831000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:46:58.164000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:47:09.967000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:47:21.945000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:47:34.491000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:47:46.579000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:47:58.863000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:48:12.238000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:48:24.176000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:48:36.074000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:48:48.343000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:49:00.544000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:49:12.776000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:49:25.058000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:49:36.829000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:49:49.014000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:50:01.523000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:50:14.090000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:50:26.178000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:50:38.473000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:50:50.605000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:51:03.004000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:51:15.336000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:51:27.635000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:51:39.784000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:51:51.838000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:52:04.044000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:52:16.066000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:52:28.307000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:52:40.453000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:52:52.510000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:53:05.009000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:53:17.272000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:53:29.391000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:53:41.645000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:53:53.377000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:54:05.629000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:54:17.790000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:54:29.854000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:54:41.906000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:54:54.318000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:55:06.309000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:55:18.577000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:55:31.048000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:55:43.215000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:55:55.239000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:56:07.538000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:56:19.372000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:56:31.338000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:56:43.644000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:56:55.820000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:57:07.997000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:57:20.130000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:57:32.129000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:57:43.944000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:57:56.653000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:58:08.523000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:58:20.480000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:58:32.796000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:58:44.863000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:58:57.105000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:59:09.865000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:59:21.959000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:59:34.051000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:59:46.265000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 11:59:58.145000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:00:10.164000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:00:22.321000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:00:34.082000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:00:46.107000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:00:58.566000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:01:10.567000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:01:22.295000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:01:34.129000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:01:46.261000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:01:58.050000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:02:10.052000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:02:22.313000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:02:34.254000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:02:46.203000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:02:58.456000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:03:10.299000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:03:22.318000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:03:34.108000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:03:46.407000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:03:58.502000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:04:10.480000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:04:22.856000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:04:34.810000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:04:46.783000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:04:59.219000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:05:11.214000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:05:23.266000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:05:35.454000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:05:47.480000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:05:59.318000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:06:11.838000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:06:24.032000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:06:36.201000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:06:48.554000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:07:01.040000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:07:13.753000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:07:26.056000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:07:37.919000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:07:49.727000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:08:02.127000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:08:14.281000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:08:26.202000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:08:39.051000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:08:51.410000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:09:03.368000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:09:15.895000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:09:28.082000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:09:40.036000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:09:52.395000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:10:04.650000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:10:17.013000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:10:29.226000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:10:41.357000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:10:53.567000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:11:05.863000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:11:17.886000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:11:29.929000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:11:42.192000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:11:54.045000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:12:05.842000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:12:16.526000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 12:12:18.279000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:12:19.741000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 12:12:21.712000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.82.34.13'),('2021-02-19 12:12:23.647000','0x23sbjw3xnoa4woqwuc324k','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.82.34.13'),('2021-02-19 12:12:56.588000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:13:18.819000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:13:19.087000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:13:29.025000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:13:39.297000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:13:49.583000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:13:59.789000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:14:10.116000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:14:20.432000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:14:30.386000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:14:41.387000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:14:51.400000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:15:01.421000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:15:08.904000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 12:15:11.426000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:15:12.987000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 12:15:18.214000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.82.34.13'),('2021-02-19 12:15:19.540000','produzione','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.82.34.13'),('2021-02-19 12:15:21.415000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:15:31.428000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:15:41.455000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:15:51.468000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:16:01.456000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:16:11.470000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:16:21.557000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:16:31.476000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:16:41.832000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:16:52.071000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:17:02.464000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:17:12.756000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:17:23.031000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:17:33.299000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:17:43.557000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:17:53.863000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:18:04.092000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:18:14.522000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.82.34.13'),('2021-02-19 12:18:19.956000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 12:18:25.357000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 12:18:27.413000','admin','Page','/coldtech/HomePage/Default.aspx','','151.82.34.13'),('2021-02-19 12:18:28.814000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.82.34.13'),('2021-02-19 13:38:46.059000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 13:38:48.766000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.82.34.13'),('2021-02-19 13:38:59.728000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.82.34.13'),('2021-02-19 13:39:03.017000','produzione','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.82.34.13'),('2021-02-19 13:53:09.295000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 13:53:13.357000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 13:53:15.357000','admin','Page','/coldtech/HomePage/Default.aspx','','151.38.15.193'),('2021-02-19 13:53:18.732000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.38.15.193'),('2021-02-19 14:03:47.607000','produzione','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 14:13:03.864000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 14:13:13.510000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 14:17:54.110000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 14:18:21.042000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 14:18:26.640000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.38.15.193'),('2021-02-19 14:18:28.634000','4gxgvqbq5f2ih3mjzuvbfr5j','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 14:18:30.166000','produzione','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.38.15.193'),('2021-02-19 14:18:30.751000','admin','Page','/coldtech/HomePage/Default.aspx','','151.38.15.193'),('2021-02-19 14:18:31.778000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.38.15.193'),('2021-02-19 14:20:20.437000','produzione','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 15:11:59.496000','admin','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 17:40:01.642000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/HomePage/Default.aspx','','151.38.15.193'),('2021-02-19 17:40:04.586000','lh10h54mdlklfootn4hsyljd','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.38.15.193'),('2021-02-19 17:40:06.821000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 17:40:14.796000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.15.193'),('2021-02-19 17:40:16.684000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.38.15.193'),('2021-02-19 17:40:17.678000','produzione','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=21/01/2021&endPeriod=21/02/2021&periodType=1','151.38.15.193'),('2021-02-20 13:24:07.557000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:24:24.667000','udhighubp0yim2hk0obgrxrw','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:24:42.417000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:24:44.823000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.38.156.72'),('2021-02-20 13:24:45.745000','produzione','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=22/01/2021&endPeriod=22/02/2021&periodType=1','151.38.156.72'),('2021-02-20 13:24:53.291000','produzione','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:24:55.276000','produzione','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:24:55.416000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:24:58.385000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:25:00.713000','admin','Page','/coldtech/HomePage/Default.aspx','','151.38.156.72'),('2021-02-20 13:25:02.042000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=22/01/2021&endPeriod=22/02/2021&periodType=1','151.38.156.72'),('2021-02-20 13:25:15.588000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.38.156.72'),('2021-02-20 13:25:21.276000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.38.156.72'),('2021-02-20 13:25:25.838000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.38.156.72'),('2021-02-20 13:25:34.479000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.38.156.72'),('2021-02-20 13:25:36.823000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:25:45.604000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:25:50.994000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:25:54.229000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:25:59.416000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:26:05.682000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:26:11.729000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:26:16.838000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:26:21.526000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:26:53.040000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.38.156.72'),('2021-02-20 13:26:55.708000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:28:42.844000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:29:00.884000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:29:06.909000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=11','151.38.156.72'),('2021-02-20 13:29:16.704000','admin','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:29:18.850000','admin','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:29:18.956000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:29:22.556000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:29:24.845000','produzione','Page','/coldtech/HomePage/Default.aspx','','151.38.156.72'),('2021-02-20 13:29:25.731000','produzione','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=22/01/2021&endPeriod=22/02/2021&periodType=1','151.38.156.72'),('2021-02-20 13:29:34.008000','produzione','Page','/coldtech/Personal/my.aspx','','151.38.156.72'),('2021-02-20 13:29:37.911000','produzione','Controller','/Personal/PersonalArea/EditDestinationURL','','151.38.156.72'),('2021-02-20 13:30:02.456000','produzione','Action','/Personal/PersonalArea/SaveDestinationURL','destUrl=~/FreeTimeMeasurement/FreeMeasurement/Execute?DepartmentId=1','151.38.156.72'),('2021-02-20 13:30:02.975000','produzione','Page','/coldtech/Personal/my.aspx','','151.38.156.72'),('2021-02-20 13:30:04.298000','produzione','Controller','/Personal/PersonalArea/EditDestinationURL','','151.38.156.72'),('2021-02-20 13:30:05.501000','produzione','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:30:07.444000','produzione','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:30:07.540000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:30:10.125000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:33:08.329000','ywutjgjc4dz1pbhw3ccfjlem','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 13:48:58.644000','bqrzz5cbvjt5iwj5t4sg2he3','Page','/coldtech/Login/login.aspx','','151.38.156.72'),('2021-02-20 14:31:20.692000','produzione','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 14:31:40.410000','2dkzyr4aih5d1arrluqgxapn','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 14:49:02.737000','23njsey3hi4wvmny2odggsdf','Page','/coldtech/login/login.aspx','','151.34.51.39'),('2021-02-20 15:15:53.788000','h35h1m1md1ew1rqpdlvoojfj','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:22:50.428000','kdaou0zbhkpk0dc4bdewfyxk','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:23:53.413000','produzione','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:23:53.522000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:24:05.241000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:24:14.381000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:24:59.506000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:25:01.522000','spedizioni','Page','/coldtech/HomePage/Default.aspx','','151.34.51.39'),('2021-02-20 15:25:02.444000','spedizioni','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=22/01/2021&endPeriod=22/02/2021&periodType=1','151.34.51.39'),('2021-02-20 15:25:35.569000','spedizioni','Page','/coldtech/Personal/my.aspx','','151.34.51.39'),('2021-02-20 15:25:39.491000','spedizioni','Controller','/Personal/PersonalArea/EditDestinationURL','','151.34.51.39'),('2021-02-20 15:25:47.631000','spedizioni','Action','/Personal/PersonalArea/SaveDestinationURL','destUrl=~FreeTimeMeasurement/FreeMeasurement/Execute?DepartmentId=2','151.34.51.39'),('2021-02-20 15:25:47.709000','spedizioni','Page','/coldtech/Personal/my.aspx','','151.34.51.39'),('2021-02-20 15:25:48.381000','spedizioni','Controller','/Personal/PersonalArea/EditDestinationURL','','151.34.51.39'),('2021-02-20 15:26:00.069000','voe5khnla5tscw2wffu3qg0h','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:11.053000','spedizioni','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:12.803000','spedizioni','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:12.944000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:15.194000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:16.944000','spedizioni','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:18.944000','spedizioni','Page','/coldtech/Personal/my.aspx','','151.34.51.39'),('2021-02-20 15:26:22.131000','spedizioni','Controller','/Personal/PersonalArea/EditDestinationURL','','151.34.51.39'),('2021-02-20 15:26:28.569000','spedizioni','Page','/coldtech/Personal/my.aspx','','151.34.51.39'),('2021-02-20 15:26:29.084000','spedizioni','Action','/Personal/PersonalArea/SaveDestinationURL','destUrl=~/FreeTimeMeasurement/FreeMeasurement/Execute?DepartmentId=2','151.34.51.39'),('2021-02-20 15:26:29.319000','spedizioni','Controller','/Personal/PersonalArea/EditDestinationURL','','151.34.51.39'),('2021-02-20 15:26:31.787000','spedizioni','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:38.131000','spedizioni','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:40.537000','spedizioni','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:40.912000','voe5khnla5tscw2wffu3qg0h','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:26:55.381000','voe5khnla5tscw2wffu3qg0h','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:47:04.304000','tzbtukxw1imkzbqor11aqwoi','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:54:01.195000','4xkrwt45jvk33di0adkejkm3','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:54:43.101000','spedizioni','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:54:45.258000','spedizioni','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:54:45.476000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:54:57.914000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 15:55:00.008000','admin','Page','/coldtech/HomePage/Default.aspx','','151.34.51.39'),('2021-02-20 15:55:01.336000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=22/01/2021&endPeriod=22/02/2021&periodType=1','151.34.51.39'),('2021-02-20 15:55:04.820000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.34.51.39'),('2021-02-20 15:55:09.929000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.34.51.39'),('2021-02-20 15:55:14.445000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.51.39'),('2021-02-20 15:55:21.945000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.51.39'),('2021-02-20 15:55:39.836000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.51.39'),('2021-02-20 15:55:43.804000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.51.39'),('2021-02-20 15:56:05.336000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.34.51.39'),('2021-02-20 15:56:06.586000','admin','Page','/coldtech/Users/managePermessi.aspx','','151.34.51.39'),('2021-02-20 15:56:09.429000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.34.51.39'),('2021-02-20 15:56:37.648000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.34.51.39'),('2021-02-20 15:56:38.554000','admin','Page','/coldtech/Admin/manageMenu.aspx','','151.34.51.39'),('2021-02-20 15:56:43.617000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=66','151.34.51.39'),('2021-02-20 15:56:46.585000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=66','151.34.51.39'),('2021-02-20 15:57:02.132000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=66','151.34.51.39'),('2021-02-20 15:57:03.085000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=66','151.34.51.39'),('2021-02-20 15:57:08.710000','admin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.34.51.39'),('2021-02-20 15:57:14.054000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=66','151.34.51.39'),('2021-02-20 15:57:29.945000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=66','151.34.51.39'),('2021-02-20 15:57:30.726000','admin','Page','/coldtech/Admin/menuShowVoce.aspx','id=66','151.34.51.39'),('2021-02-20 15:58:29.616000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.34.51.39'),('2021-02-20 15:58:30.413000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.34.51.39'),('2021-02-20 15:58:35.272000','admin','Page','/coldtech/Users/editUser.aspx','id=timeadmin','151.34.51.39'),('2021-02-20 15:58:43.944000','admin','Page','/coldtech/Users/editUser.aspx','id=timeadmin','151.34.51.39'),('2021-02-20 15:58:50.835000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.51.39'),('2021-02-20 15:58:53.272000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:00.507000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:03.288000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:07.788000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:10.632000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:16.179000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:18.976000','admin','Page','/coldtech/Admin/MenuGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:33.241000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.51.39'),('2021-02-20 15:59:34.757000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=11','151.34.51.39'),('2021-02-20 15:59:44.632000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.51.39'),('2021-02-20 15:59:46.382000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:53.241000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:56.601000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 15:59:59.663000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 16:00:03.507000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 16:00:06.726000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 16:00:09.413000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 16:00:27.710000','admin','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:00:29.366000','admin','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:00:29.477000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:00:35.054000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:00:37.335000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.34.51.39'),('2021-02-20 16:00:39.569000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=22/01/2021&endPeriod=22/02/2021&periodType=1','151.34.51.39'),('2021-02-20 16:00:43.866000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.34.51.39'),('2021-02-20 16:00:45.226000','timeadmin','Page','/coldtech/Personal/my.aspx','','151.34.51.39'),('2021-02-20 16:00:48.476000','timeadmin','Controller','/Personal/PersonalArea/EditDestinationURL','','151.34.51.39'),('2021-02-20 16:01:05.257000','5dk0qmpdshhxb35j55fhytc5','Page','/coldtech/Personal/my.aspx','','151.34.51.39'),('2021-02-20 16:01:06.619000','5dk0qmpdshhxb35j55fhytc5','Controller','/Personal/PersonalArea/EditDestinationURL','','151.34.51.39'),('2021-02-20 16:01:07.866000','5dk0qmpdshhxb35j55fhytc5','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:01:10.788000','5dk0qmpdshhxb35j55fhytc5','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:01:15.007000','5dk0qmpdshhxb35j55fhytc5','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:01:17.148000','admin','Page','/coldtech/HomePage/Default.aspx','','151.34.51.39'),('2021-02-20 16:01:18.054000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=22/01/2021&endPeriod=22/02/2021&periodType=1','151.34.51.39'),('2021-02-20 16:01:23.304000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.51.39'),('2021-02-20 16:01:53.335000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.34.51.39'),('2021-02-20 16:01:57.679000','admin','Page','/coldtech/Users/manageGruppi.aspx','','151.34.51.39'),('2021-02-20 16:02:02.866000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 16:02:28.366000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 16:02:33.647000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 16:02:36.616000','admin','Page','/coldtech/Users/PermessiGruppi.aspx','ID=12','151.34.51.39'),('2021-02-20 16:02:43.460000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.34.51.39'),('2021-02-20 16:02:56.241000','timeadmin','Page','/coldtech/Personal/my.aspx','','151.34.51.39'),('2021-02-20 16:02:59.022000','timeadmin','Controller','/Personal/PersonalArea/EditDestinationURL','','151.34.51.39'),('2021-02-20 16:03:05.913000','timeadmin','Page','/coldtech/Personal/my.aspx','','151.34.51.39'),('2021-02-20 16:03:16.147000','timeadmin','Page','/coldtech/Personal/my.aspx','','151.34.51.39'),('2021-02-20 16:03:34.413000','timeadmin','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:03:36.007000','timeadmin','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:03:36.100000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:03:38.725000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.34.51.39'),('2021-02-20 16:03:40.725000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.34.51.39'),('2021-02-20 16:03:41.741000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=22/01/2021&endPeriod=22/02/2021&periodType=1','151.34.51.39'),('2021-02-21 10:02:03.363000','rjkkbjowili4folhmphsgpj4','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 10:02:08.784000','rjkkbjowili4folhmphsgpj4','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:00:06.983000','bjuhidheutxoopf1sdz15j1x','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:00:12.371000','bjuhidheutxoopf1sdz15j1x','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:02:39.065000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:02:44.044000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:07:49.315000','produzione','Page','/coldtech/Personal/my.aspx','','151.68.127.250'),('2021-02-21 12:07:53.647000','produzione','Controller','/Personal/PersonalArea/EditDestinationURL','','151.68.127.250'),('2021-02-21 12:07:59.805000','produzione','Page','/coldtech/Personal/my.aspx','','151.68.127.250'),('2021-02-21 12:08:03.601000','produzione','Page','/coldtech/Personal/my.aspx','','151.68.127.250'),('2021-02-21 12:08:08.422000','produzione','Page','/coldtech/Personal/my.aspx','','151.68.127.250'),('2021-02-21 12:08:12.324000','produzione','Page','/coldtech/Personal/my.aspx','','151.68.127.250'),('2021-02-21 12:08:15.905000','produzione','Controller','/Personal/PersonalArea/EditDestinationURL','','151.68.127.250'),('2021-02-21 12:09:05.377000','produzione','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:09:08.242000','produzione','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:09:08.363000','bjuhidheutxoopf1sdz15j1x','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:09:11.401000','bjuhidheutxoopf1sdz15j1x','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:17:02.860000','gq2jcpufwpiv23l1dcujczb5','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:17:28.553000','gq2jcpufwpiv23l1dcujczb5','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:25:06.011000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:25:10.794000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:41:17.717000','q10w3f23eyk03sp4ylwmo2zx','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:41:21.253000','q10w3f23eyk03sp4ylwmo2zx','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:41:53.389000','hx4xrt3zkwmxixxbioq4s5iu','Page','/coldtech/HomePage/Default.aspx','','151.68.127.250'),('2021-02-21 12:41:54.729000','hx4xrt3zkwmxixxbioq4s5iu','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=23/01/2021&endPeriod=23/02/2021&periodType=1','151.68.127.250'),('2021-02-21 12:41:56.123000','hx4xrt3zkwmxixxbioq4s5iu','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:42:03.285000','hx4xrt3zkwmxixxbioq4s5iu','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:42:06.196000','hx4xrt3zkwmxixxbioq4s5iu','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:42:08.627000','admin','Page','/coldtech/HomePage/Default.aspx','','151.68.127.250'),('2021-02-21 12:42:09.796000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=23/01/2021&endPeriod=23/02/2021&periodType=1','151.68.127.250'),('2021-02-21 12:42:23.518000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:42:29.176000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:42:43.132000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:42:43.896000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:42:49.291000','admin','Page','/coldtech/Users/editUser.aspx','id=op3','151.68.127.250'),('2021-02-21 12:42:55.097000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:42:58.126000','admin','Page','/coldtech/Users/editUser.aspx','id=op3','151.68.127.250'),('2021-02-21 12:43:15.736000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:43:16.747000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:43:21.968000','admin','Page','/coldtech/Users/editUser.aspx','id=op4','151.68.127.250'),('2021-02-21 12:43:28.060000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:43:30.667000','admin','Page','/coldtech/Users/editUser.aspx','id=op4','151.68.127.250'),('2021-02-21 12:43:45.864000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:43:46.761000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:43:51.920000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:44:07.888000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:44:08.750000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:44:15.550000','admin','Page','/coldtech/Users/editUser.aspx','id=op5','151.68.127.250'),('2021-02-21 12:44:21.912000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:44:23.295000','admin','Page','/coldtech/Users/editUser.aspx','id=op6','151.68.127.250'),('2021-02-21 12:44:30.285000','admin','Page','/coldtech/Users/editUser.aspx','id=op5','151.68.127.250'),('2021-02-21 12:44:33.205000','admin','Page','/coldtech/Users/editUser.aspx','id=op6','151.68.127.250'),('2021-02-21 12:44:56.935000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:44:57.749000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:45:04.755000','admin','Page','/coldtech/Users/editUser.aspx','id=op7','151.68.127.250'),('2021-02-21 12:45:10.832000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:45:13.737000','admin','Page','/coldtech/Users/editUser.aspx','id=op7','151.68.127.250'),('2021-02-21 12:45:30.777000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:45:31.560000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:45:45.322000','admin','Page','/coldtech/Users/editUser.aspx','id=op8','151.68.127.250'),('2021-02-21 12:45:51.363000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:45:55.281000','admin','Page','/coldtech/Users/editUser.aspx','id=op8','151.68.127.250'),('2021-02-21 12:46:12.213000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:46:13.022000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:46:20.306000','admin','Page','/coldtech/Users/editUser.aspx','id=op9','151.68.127.250'),('2021-02-21 12:46:26.181000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:46:30.958000','admin','Page','/coldtech/Users/editUser.aspx','id=op9','151.68.127.250'),('2021-02-21 12:46:43.245000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:46:43.398000','bit1mfsq5bldmlxtimozdx34','Page','/coldtech/HomePage/Default.aspx','','151.68.127.250'),('2021-02-21 12:46:44.068000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.68.127.250'),('2021-02-21 12:46:44.352000','j4hyzpvxpkgmx3hprlnmsa1l','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=23/01/2021&endPeriod=23/02/2021&periodType=1','151.68.127.250'),('2021-02-21 12:46:53.254000','admin','Page','/coldtech/Users/editUser.aspx','id=op10','151.68.127.250'),('2021-02-21 12:47:02.205000','admin','Page','/coldtech/Users/editUser.aspx','id=op10','151.68.127.250'),('2021-02-21 12:47:29.323000','produzione','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:47:31.607000','produzione','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:47:31.695000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:47:36.726000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 12:47:41.798000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.68.127.250'),('2021-02-21 12:47:43.006000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=23/01/2021&endPeriod=23/02/2021&periodType=1','151.68.127.250'),('2021-02-21 13:11:55.557000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 13:12:13.840000','q10w3f23eyk03sp4ylwmo2zx','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 13:12:17.596000','q10w3f23eyk03sp4ylwmo2zx','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 13:15:04.049000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 13:21:48.711000','produzione','Page','/coldtech/Login/login.aspx','','151.68.127.250'),('2021-02-21 15:49:43.423000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.35.26'),('2021-02-21 15:50:35.605000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.18.35.26'),('2021-02-21 16:27:04.169000','sqtgelrcb0ijzmogr3g1ch3k','Page','/coldtech/Login/login.aspx','','151.18.35.26'),('2021-02-21 16:43:32.990000','1mmnrrk4sjlalkel52x0pbaj','Page','/coldtech/Login/login.aspx','','151.18.35.26'),('2021-02-21 16:44:49.763000','1mmnrrk4sjlalkel52x0pbaj','Page','/coldtech/Login/login.aspx','','151.18.35.26'),('2021-02-21 16:44:52.542000','1mmnrrk4sjlalkel52x0pbaj','Page','/coldtech/Login/login.aspx','','151.18.35.26'),('2021-02-21 19:12:28.708000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/HomePage/Default.aspx','','151.18.17.180'),('2021-02-21 19:12:40.974000','lh10h54mdlklfootn4hsyljd','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=23/01/2021&endPeriod=23/02/2021&periodType=1','151.18.17.180'),('2021-02-21 19:12:43.802000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.18.17.180'),('2021-02-21 19:12:50.395000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.18.17.180'),('2021-02-21 19:13:07.895000','wuxert5p5bpbrtdobaux1lyv','Page','/coldtech/HomePage/Default.aspx','','151.18.17.180'),('2021-02-21 19:13:08.536000','wuxert5p5bpbrtdobaux1lyv','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=23/01/2021&endPeriod=23/02/2021&periodType=1','151.18.17.180'),('2021-02-21 19:13:43.020000','wuxert5p5bpbrtdobaux1lyv','Page','/coldtech/Login/login.aspx','','151.18.17.180'),('2021-02-21 19:13:47.645000','wuxert5p5bpbrtdobaux1lyv','Page','/coldtech/Login/login.aspx','','151.18.17.180'),('2021-02-21 19:13:51.911000','admin','Page','/coldtech/HomePage/Default.aspx','','151.18.17.180'),('2021-02-21 19:13:54.161000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=23/01/2021&endPeriod=23/02/2021&periodType=1','151.18.17.180'),('2021-02-21 19:14:01.177000','admin','Page','/coldtech/Login/login.aspx','','151.18.17.180'),('2021-02-21 19:14:02.802000','admin','Page','/coldtech/Login/login.aspx','','151.18.17.180'),('2021-02-21 19:14:02.895000','wuxert5p5bpbrtdobaux1lyv','Page','/coldtech/Login/login.aspx','','151.18.17.180'),('2021-02-21 19:14:34.317000','wuxert5p5bpbrtdobaux1lyv','Page','/coldtech/Login/login.aspx','','151.18.17.180'),('2021-02-21 19:14:38.442000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.18.17.180'),('2021-02-21 19:14:38.989000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=23/01/2021&endPeriod=23/02/2021&periodType=1','151.18.17.180'),('2021-02-22 06:23:35.970000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/HomePage/Default.aspx','','151.68.3.217'),('2021-02-22 06:23:38.892000','lh10h54mdlklfootn4hsyljd','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.68.3.217'),('2021-02-22 06:23:40.298000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.3.217'),('2021-02-22 06:23:43.439000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.3.217'),('2021-02-22 06:25:33.329000','1r4gwqyq4523hvmoqf10jzq2','Page','/coldtech/HomePage/Default.aspx','','151.68.3.217'),('2021-02-22 06:25:33.783000','1r4gwqyq4523hvmoqf10jzq2','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.68.3.217'),('2021-02-22 06:25:37.876000','1r4gwqyq4523hvmoqf10jzq2','Page','/coldtech/Login/login.aspx','','151.68.3.217'),('2021-02-22 06:25:57.001000','1r4gwqyq4523hvmoqf10jzq2','Page','/coldtech/Login/login.aspx','','151.68.3.217'),('2021-02-22 06:26:02.829000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.68.3.217'),('2021-02-22 06:26:04.533000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.68.3.217'),('2021-02-22 07:38:06.381000','gpuypk2vai3qh1tgvgmr5ppz','Page','/coldtech/Login/login.aspx','','37.161.134.223'),('2021-02-22 07:38:26.778000','gpuypk2vai3qh1tgvgmr5ppz','Page','/coldtech/Login/login.aspx','','37.161.134.223'),('2021-02-22 07:44:30.822000','sqtgelrcb0ijzmogr3g1ch3k','Page','/coldtech/Login/login.aspx','','37.161.3.165'),('2021-02-22 07:44:36.461000','sqtgelrcb0ijzmogr3g1ch3k','Page','/coldtech/Login/login.aspx','','37.161.3.165'),('2021-02-22 07:46:32.946000','bxdt4uk4m1ksldphwqkwhnci','Page','/coldtech/Login/login.aspx','','37.163.162.227'),('2021-02-22 07:46:40.525000','bxdt4uk4m1ksldphwqkwhnci','Page','/coldtech/Login/login.aspx','','37.163.162.227'),('2021-02-22 07:49:42.975000','mvpy34mzbn1fv5o1e1m0z42t','Page','/coldtech/Login/login.aspx','','37.160.177.234'),('2021-02-22 10:34:14.955000','produzione','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 10:34:20.065000','produzione','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 10:34:20.158000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 10:34:28.956000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 10:34:34.752000','admin','Page','/coldtech/HomePage/Default.aspx','','151.38.137.85'),('2021-02-22 10:34:36.674000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.38.137.85'),('2021-02-22 10:34:54.759000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.137.85'),('2021-02-22 10:35:19.860000','admin','Page','/coldtech/Users/manageUsers.aspx','','151.38.137.85'),('2021-02-22 10:36:35.440000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.137.85'),('2021-02-22 10:36:40.703000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0','151.38.137.85'),('2021-02-22 10:36:51.352000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0','151.38.137.85'),('2021-02-22 10:36:53.739000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:36:55.289000','admin','Controller','/Products/ProductParameters/Index','processID=0&processRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:37:03.142000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=138&posy=145&variante=4','151.38.137.85'),('2021-02-22 10:37:04.132000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=6&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:37:05.686000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=6&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:37:06.719000','admin','Controller','/Products/Products/TaskParametersList','TaskID=6&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:37:08.264000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=6&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:37:09.311000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=6&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:37:12.229000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:37:13.032000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=6&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:37:26.113000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:37:28.557000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=6&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:37:32.717000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=6&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:37:33.339000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=6&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:37:38.447000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:37:39.958000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=138&posy=221&variante=4','151.38.137.85'),('2021-02-22 10:37:51.122000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:37:51.609000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=7&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:37:52.157000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&act=updatepos&posx=368&posy=226&variante=4','151.38.137.85'),('2021-02-22 10:37:52.889000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=7&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:37:53.910000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=7&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:37:54.915000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=7&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:37:55.920000','admin','Controller','/Products/Products/TaskParametersList','TaskID=7&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:37:57.496000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=7&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:37:59.027000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=7&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:03.534000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:38:04.169000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=7&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:04.877000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=7&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:06.270000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=6&revTaskID=0&variante=4','151.38.137.85'),('2021-02-22 10:38:09.704000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=6&revTaskID=0&variante=4','151.38.137.85'),('2021-02-22 10:38:16.469000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:38:19.153000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=8&act=updatepos&posx=571&posy=247&variante=4','151.38.137.85'),('2021-02-22 10:38:20.113000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=8&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:20.844000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=8&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:21.858000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=8&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:38:22.860000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=8&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:38:23.870000','admin','Controller','/Products/Products/TaskParametersList','TaskID=8&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:25.623000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=8&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:38:28.810000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:38:29.701000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=8&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:32.218000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=8&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:32.833000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=8&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:41.738000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:38:45.219000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=9&act=updatepos&posx=782&posy=226&variante=4','151.38.137.85'),('2021-02-22 10:38:45.686000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:46.335000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:47.360000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=9&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:38:48.370000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:38:49.374000','admin','Controller','/Products/Products/TaskParametersList','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:38:50.903000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:38:54.378000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:38:54.987000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:00.639000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:01.461000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:04.638000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=8&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:06.894000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:39:07.811000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=8&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:08.815000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=8&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:39:09.826000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=8&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:39:10.828000','admin','Controller','/Products/Products/TaskParametersList','TaskID=8&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:12.501000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=8&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:39:13.526000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=4','151.38.137.85'),('2021-02-22 10:39:19.238000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:39:20.129000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=4','151.38.137.85'),('2021-02-22 10:39:25.181000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=4','151.38.137.85'),('2021-02-22 10:39:26.632000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:27.657000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:28.672000','admin','Controller','/Products/Products/TaskParametersList','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:30.198000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=9&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:39:31.227000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:39:32.413000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:39:35.148000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:39:39.808000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=9&TaskRev=0&VariantID=4','151.38.137.85'),('2021-02-22 10:39:42.345000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=8&revTaskID=0&variante=4','151.38.137.85'),('2021-02-22 10:39:45.615000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=8&revTaskID=0&variante=4','151.38.137.85'),('2021-02-22 10:39:47.928000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:40:15.531000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:40:29.505000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:40:41.441000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:40:53.424000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:05.466000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:15.025000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:20.562000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:22.703000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:27.442000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:29.282000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:38.334000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:40.035000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:43.832000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:45.399000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=0&variante=4&repID=1','151.38.137.85'),('2021-02-22 10:41:55.815000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:41:57.372000','admin','Controller','/Products/ProductParameters/Index','processID=0&processRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:42:03.930000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:42:04.714000','admin','Controller','/Products/ProductParameters/Index','processID=0&processRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:42:16.447000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:42:19.643000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:42:22.156000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:42:23.864000','admin','Controller','/Products/ProductParameters/Index','processID=0&processRev=0&variantID=4','151.38.137.85'),('2021-02-22 10:42:35.585000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=0&variante=4','151.38.137.85'),('2021-02-22 10:42:43.050000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.137.85'),('2021-02-22 10:43:36.148000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1','151.38.137.85'),('2021-02-22 10:43:42.678000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:43:44.114000','admin','Controller','/Products/ProductParameters/Index','processID=1&processRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:43:55.754000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:43:56.443000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=10&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:43:57.130000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=10&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:43:58.138000','admin','Controller','/Products/Products/TaskParametersList','TaskID=10&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:43:59.728000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=10&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:44:00.731000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=10&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:44:02.016000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=10&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:44:03.110000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=10&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:44:05.387000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=10&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:44:05.866000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=10&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:44:08.569000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:44:09.345000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=123&posy=343&variante=3','151.38.137.85'),('2021-02-22 10:44:11.138000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=123&posy=343&variante=3','151.38.137.85'),('2021-02-22 10:44:21.181000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:44:28.685000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=11&act=updatepos&posx=102&posy=486&variante=3','151.38.137.85'),('2021-02-22 10:44:36.047000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:44:36.861000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=11&act=updatepos&posx=498&posy=183&variante=3','151.38.137.85'),('2021-02-22 10:44:37.652000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=11&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:44:38.311000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=11&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:44:39.389000','admin','Controller','/Products/Products/TaskParametersList','TaskID=11&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:44:40.892000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=11&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:44:41.905000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=11&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:44:42.785000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=11&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:44:48.171000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:44:52.270000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=10&act=updatepos&posx=196&posy=223&variante=3','151.38.137.85'),('2021-02-22 10:44:55.323000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=118&posy=420&variante=3','151.38.137.85'),('2021-02-22 10:44:56.463000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=10&act=updatepos&posx=124&posy=252&variante=3','151.38.137.85'),('2021-02-22 10:45:01.121000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:45:09.916000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=11&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:45:10.873000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=11&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:45:13.537000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:45:13.966000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=11&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:45:14.845000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=11&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:45:15.950000','admin','Controller','/Products/Products/TaskParametersList','TaskID=11&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:45:17.500000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=11&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:45:26.895000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:45:39.369000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:45:40.136000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=11&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:45:43.149000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=11&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:45:43.698000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=11&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:45:51.958000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:45:53.262000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=122&posy=507&variante=3','151.38.137.85'),('2021-02-22 10:45:55.752000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=10&act=updatepos&posx=98&posy=386&variante=3','151.38.137.85'),('2021-02-22 10:45:57.378000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&act=updatepos&posx=98&posy=237&variante=3','151.38.137.85'),('2021-02-22 10:46:04.468000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:46:05.001000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:46:05.576000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=12&act=updatepos&posx=520&posy=384&variante=3','151.38.137.85'),('2021-02-22 10:46:06.087000','admin','Controller','/Products/Products/TaskParametersList','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:46:07.604000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:46:08.651000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=12&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:46:09.660000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=12&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:46:10.830000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=12&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:46:16.770000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:46:26.343000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:46:28.960000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:46:30.553000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:46:31.058000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:46:41.727000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:46:46.949000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=13&act=updatepos&posx=744&posy=264&variante=3','151.38.137.85'),('2021-02-22 10:46:47.451000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=13&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:46:47.973000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=13&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:46:49.002000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=13&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:46:49.878000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=13&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:46:51.014000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=13&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:46:52.026000','admin','Controller','/Products/Products/TaskParametersList','TaskID=13&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:46:55.001000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:47:00.204000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=13&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:47:03.492000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=13&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:47:04.324000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=13&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:47:07.149000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:47:19.383000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:47:23.291000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=8&act=updatepos&posx=784&posy=436&variante=3','151.38.137.85'),('2021-02-22 10:47:26.155000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=12&act=updatepos&posx=414&posy=328&variante=3','151.38.137.85'),('2021-02-22 10:47:27.841000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=11&act=updatepos&posx=443&posy=143&variante=3','151.38.137.85'),('2021-02-22 10:47:32.494000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:47:40.633000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=14&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:47:41.508000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=14&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:47:42.529000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=14&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:47:45.827000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:47:46.572000','admin','Controller','/Products/Products/TaskParametersList','TaskID=14&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:47:48.098000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=14&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:47:49.058000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=14&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:47:50.823000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=14&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:47:53.857000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=14&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:47:54.685000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=14&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:47:58.185000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:48:00.292000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=14&act=updatepos&posx=882&posy=340&variante=3','151.38.137.85'),('2021-02-22 10:48:01.410000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=8&act=updatepos&posx=687&posy=447&variante=3','151.38.137.85'),('2021-02-22 10:48:03.336000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=13&act=updatepos&posx=639&posy=257&variante=3','151.38.137.85'),('2021-02-22 10:48:05.581000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=14&act=updatepos&posx=793&posy=327&variante=3','151.38.137.85'),('2021-02-22 10:48:06.641000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=8&act=updatepos&posx=669&posy=443&variante=3','151.38.137.85'),('2021-02-22 10:48:10.768000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:48:23.234000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:48:23.825000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=9&act=updatepos&posx=950&posy=141&variante=3','151.38.137.85'),('2021-02-22 10:48:24.652000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=9&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:25.288000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=9&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:26.315000','admin','Controller','/Products/Products/TaskParametersList','TaskID=9&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:27.844000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:48:28.858000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=9&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:48:29.888000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=9&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:30.427000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=9&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:31.512000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:48:33.407000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=8&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:34.054000','admin','Controller','/Products/Products/TaskParametersList','TaskID=8&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:35.390000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=8&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:48:38.064000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:48:38.597000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=8&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:39.605000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=8&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:48:40.434000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=8&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:48:41.284000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=8&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:41.818000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=8&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:50.184000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:48:50.658000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=15&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:51.637000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=15&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:52.659000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=15&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:48:53.682000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=15&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:48:55.145000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=15&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 10:48:56.191000','admin','Controller','/Products/Products/TaskParametersList','TaskID=15&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:48:57.377000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=15&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:49:00.658000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=15&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:49:02.941000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:49:03.575000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=15&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 10:49:05.780000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=15&act=updatepos&posx=1000&posy=457&variante=3','151.38.137.85'),('2021-02-22 10:49:07.607000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&act=updatepos&posx=105&posy=99&variante=3','151.38.137.85'),('2021-02-22 10:49:10.049000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=10&act=updatepos&posx=116&posy=247&variante=3','151.38.137.85'),('2021-02-22 10:49:12.449000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=109&posy=412&variante=3','151.38.137.85'),('2021-02-22 10:49:15.315000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:49:15.811000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 10:49:24.456000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 10:49:27.816000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:49:28.609000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 10:49:35.851000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 10:49:39.943000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:49:40.874000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=8&act=updatepos&posx=442&posy=490&variante=3','151.38.137.85'),('2021-02-22 10:49:46.741000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=10&act=updatepos&posx=209&posy=85&variante=3','151.38.137.85'),('2021-02-22 10:49:50.099000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&act=updatepos&posx=114&posy=196&variante=3','151.38.137.85'),('2021-02-22 10:49:52.956000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:49:53.475000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 10:49:54.786000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&act=updatepos&posx=114&posy=263&variante=3','151.38.137.85'),('2021-02-22 10:49:55.708000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=10&act=updatepos&posx=142&posy=81&variante=3','151.38.137.85'),('2021-02-22 10:50:05.274000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:50:17.876000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:50:30.765000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:50:43.805000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:50:56.729000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:51:10.010000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:51:22.872000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:51:35.770000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:51:48.930000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:52:01.780000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:52:14.438000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:52:27.779000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:52:41.292000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:52:54.097000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:53:07.373000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:53:21.576000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:53:34.738000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:53:47.964000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:54:00.740000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:54:14.098000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:54:27.017000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:54:39.845000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:54:52.989000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:55:06.265000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:55:19.052000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:55:31.949000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:55:44.873000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:55:57.604000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:56:10.642000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:56:23.892000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:56:36.969000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:56:49.866000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:57:02.623000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:57:15.961000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:57:28.795000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:57:41.977000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:58:28.904000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:58:54.066000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:58:54.357000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:58:59.020000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:59:10.139000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:59:21.033000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:59:32.048000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:59:42.133000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 10:59:53.050000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:00:04.007000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:00:15.185000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:00:30.842000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:01:23.945000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:01:34.003000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:02:42.110000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:02:43.074000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:02:45.518000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 11:02:48.954000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 11:02:57.657000','admin','Page','/coldtech/HomePage/Default.aspx','','151.38.137.85'),('2021-02-22 11:02:59.967000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:03:03.294000','admin','Controller','/Products/ProductParameters/Index','processID=1&processRev=0&variantID=3','151.38.137.85'),('2021-02-22 11:03:06.169000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:03:07.381000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.38.137.85'),('2021-02-22 11:03:13.038000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:03:25.808000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:03:38.474000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:06:43.991000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:06:56.747000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:07:10.143000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:07:22.767000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:07:35.630000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:07:48.714000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:08:01.577000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:08:14.606000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:08:27.620000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:08:40.560000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:08:53.711000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:09:06.847000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:09:19.887000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:09:32.605000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:09:45.820000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:09:58.558000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:10:11.579000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:10:24.961000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:10:37.803000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:10:52.614000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:11:05.597000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:11:18.721000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:11:31.939000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:11:44.617000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:11:57.697000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:12:10.463000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:12:23.866000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:12:36.492000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:12:49.750000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:13:02.881000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:13:15.056000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:13:16.182000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=10&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:13:24.272000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=10&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:13:27.653000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:13:31.963000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 11:13:33.917000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 11:13:34.955000','admin','Controller','/Products/Products/TaskParametersList','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 11:13:36.518000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=12&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 11:13:37.538000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=12&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 11:13:38.370000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=12&TaskRev=0&variantID=3','151.38.137.85'),('2021-02-22 11:13:40.797000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:13:41.361000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=12&TaskRev=0&VariantID=3','151.38.137.85'),('2021-02-22 11:13:53.739000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:14:06.560000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:14:19.664000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:14:32.671000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:14:45.698000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:14:59.275000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:15:12.794000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:15:25.940000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:15:38.927000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:15:51.935000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:16:04.594000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:16:17.732000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:16:30.553000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:16:43.844000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:16:56.631000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:17:09.648000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:17:22.748000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:17:36.019000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:17:49.892000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:18:02.651000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:18:15.855000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:18:28.645000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:18:41.830000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:18:55.225000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:19:07.710000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:19:21.069000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:19:33.741000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:19:46.652000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:20:00.112000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:20:13.635000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:20:26.905000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:20:39.641000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:20:52.759000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:21:05.630000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:21:18.788000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:21:31.837000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:21:44.861000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:21:57.605000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:22:10.477000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:22:24.007000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:22:36.734000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:22:49.719000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:23:02.789000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:23:15.704000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:23:28.601000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:23:41.923000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:23:54.572000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:24:07.576000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:24:20.712000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:24:33.611000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:24:46.695000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:24:59.701000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:25:12.668000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:25:25.707000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:25:38.804000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:25:51.558000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:26:04.582000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:26:17.507000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:26:30.757000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:26:43.641000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:26:56.552000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:27:09.957000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:27:22.736000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:27:35.605000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:27:48.578000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:28:01.720000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:28:14.633000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:28:27.515000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:28:40.669000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:28:53.593000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:29:06.718000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:29:19.694000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:29:33.021000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:29:45.797000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:29:58.847000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:30:11.912000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:30:25.548000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:30:38.588000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:30:51.636000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:31:04.687000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:31:17.609000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:31:31.080000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:31:44.564000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:31:57.723000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:32:10.615000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:32:23.755000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:32:36.863000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:32:49.590000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:33:02.670000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:33:15.678000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:33:28.511000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:33:41.659000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:33:54.636000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:34:07.583000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:34:20.787000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:34:33.684000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:34:46.720000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:34:59.768000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:35:12.640000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:35:25.766000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:35:38.666000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:35:51.488000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:36:04.751000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:36:17.554000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:36:30.538000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:36:43.573000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:36:56.674000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:37:09.646000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:37:22.637000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:37:35.846000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:37:48.590000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:38:02.124000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:38:14.565000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:38:16.909000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:38:21.017000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&delID=12&verso=succ&act=delprecedenze&variante=3','151.38.137.85'),('2021-02-22 11:38:23.627000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:38:26.709000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:38:27.359000','admin','Controller','/Products/ProductParameters/Index','processID=1&processRev=0&variantID=3','151.38.137.85'),('2021-02-22 11:38:37.628000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:38:38.981000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:38:42.894000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&delID=11&verso=succ&act=delprecedenze&variante=3','151.38.137.85'),('2021-02-22 11:38:45.464000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:38:46.559000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=10&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:38:47.919000','admin','Controller','/Products/ProductParameters/Index','processID=1&processRev=0&variantID=3','151.38.137.85'),('2021-02-22 11:38:51.821000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=10&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:38:58.451000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:39:00.170000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=11&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:05.542000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=11&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:06.880000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=12&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:10.878000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:39:11.988000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=12&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:14.688000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=8&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:18.715000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=8&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:20.111000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=6&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:23.535000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:39:24.527000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=6&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:27.680000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:32.788000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:35.881000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:39:36.829000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=124&posy=614&variante=3','151.38.137.85'),('2021-02-22 11:39:38.979000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&act=updatepos&posx=114&posy=426&variante=3','151.38.137.85'),('2021-02-22 11:39:40.707000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=11&act=updatepos&posx=426&posy=79&variante=3','151.38.137.85'),('2021-02-22 11:39:42.085000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=12&act=updatepos&posx=436&posy=235&variante=3','151.38.137.85'),('2021-02-22 11:39:43.935000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=13&act=updatepos&posx=657&posy=160&variante=3','151.38.137.85'),('2021-02-22 11:39:47.004000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=13&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:50.114000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:39:51.470000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=13&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:39:53.728000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=8&act=updatepos&posx=448&posy=384&variante=3','151.38.137.85'),('2021-02-22 11:39:55.059000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&act=updatepos&posx=117&posy=384&variante=3','151.38.137.85'),('2021-02-22 11:39:58.791000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=15&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:40:02.548000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:40:04.994000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=15&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:40:06.600000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=14&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:40:10.091000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=9&act=updatepos&posx=1056&posy=194&variante=3','151.38.137.85'),('2021-02-22 11:40:15.386000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:40:18.175000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=14&revTaskID=0&variante=3','151.38.137.85'),('2021-02-22 11:40:22.274000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=10&act=updatepos&posx=90&posy=116&variante=3','151.38.137.85'),('2021-02-22 11:40:23.754000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=11&act=updatepos&posx=320&posy=90&variante=3','151.38.137.85'),('2021-02-22 11:40:26.131000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=12&act=updatepos&posx=332&posy=230&variante=3','151.38.137.85'),('2021-02-22 11:40:29.008000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=1&variante=3','151.38.137.85'),('2021-02-22 11:40:29.989000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=13&act=updatepos&posx=578&posy=160&variante=3','151.38.137.85'),('2021-02-22 11:40:31.369000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=14&act=updatepos&posx=779&posy=252&variante=3','151.38.137.85'),('2021-02-22 11:40:33.730000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=15&act=updatepos&posx=914&posy=363&variante=3','151.38.137.85'),('2021-02-22 11:40:35.279000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=9&act=updatepos&posx=1061&posy=355&variante=3','151.38.137.85'),('2021-02-22 11:41:06.767000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:12.192000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:14.736000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:18.415000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:21.213000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:25.859000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:28.636000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:35.267000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:38.068000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:41.894000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:45.027000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:49.305000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:41:52.221000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:00.069000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:03.343000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:05.613000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:09.658000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:12.560000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:19.431000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:21.368000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:23.190000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:26.188000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:28.078000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:38.581000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:43.803000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:42:56.172000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:43:08.136000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:43:20.192000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:43:32.706000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:43:44.628000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:43:56.258000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:44:08.090000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:44:20.256000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:44:32.349000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:44:44.455000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:44:56.437000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:45:09.384000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:45:21.451000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:45:33.460000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:45:45.347000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:45:57.417000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:46:09.504000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:46:21.588000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:46:33.649000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:46:45.481000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:46:57.518000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:47:09.639000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:47:21.504000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:47:34.528000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:47:46.514000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:47:58.505000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:48:12.579000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:48:24.648000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:48:36.529000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:48:48.352000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:49:00.529000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:49:12.480000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:49:24.702000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:49:37.517000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:49:49.437000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:50:01.614000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:50:14.577000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:50:26.574000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:50:38.512000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:50:50.410000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:51:02.749000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:51:15.541000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:51:27.482000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:51:39.555000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:51:52.338000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=1&variante=3&repID=1','151.38.137.85'),('2021-02-22 11:52:37.403000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.137.85'),('2021-02-22 12:06:47.735000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.137.85'),('2021-02-22 12:06:54.080000','admin','Page','/coldtech/Processi/AddMacroProcesso.aspx','','151.38.137.85'),('2021-02-22 12:06:54.877000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.137.85'),('2021-02-22 12:07:03.025000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16','151.38.137.85'),('2021-02-22 12:07:11.128000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16','151.38.137.85'),('2021-02-22 12:07:13.438000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:07:15.369000','admin','Controller','/Products/ProductParameters/Index','processID=16&processRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:07:18.611000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:07:19.509000','admin','Controller','/Products/ProductParameters/Index','processID=16&processRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:07:31.122000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:07:41.320000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:07:43.426000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:07:45.413000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:07:47.036000','admin','Controller','/Products/ProductParameters/Index','processID=16&processRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:07:56.895000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=101&posy=180&variante=5','151.38.137.85'),('2021-02-22 12:07:57.361000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=6&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:07:58.020000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=6&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:07:59.054000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=6&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:08:01.711000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:08:02.612000','admin','Controller','/Products/Products/TaskParametersList','TaskID=6&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:04.173000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=6&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:08:04.954000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=6&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:08:06.168000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=6&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:07.003000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=6&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:13.728000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:08:14.378000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=7&act=updatepos&posx=133&posy=328&variante=5','151.38.137.85'),('2021-02-22 12:08:15.428000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=7&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:16.128000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=7&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:17.152000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=7&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:08:18.164000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=7&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:08:19.057000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=7&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:08:20.179000','admin','Controller','/Products/Products/TaskParametersList','TaskID=7&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:23.361000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=7&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:23.828000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=7&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:26.432000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:08:39.619000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:08:42.630000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=8&act=updatepos&posx=336&posy=350&variante=5','151.38.137.85'),('2021-02-22 12:08:43.939000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=8&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:44.763000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=8&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:45.790000','admin','Controller','/Products/Products/TaskParametersList','TaskID=8&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:47.297000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=8&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:08:48.298000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=8&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:08:49.385000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=8&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:08:52.227000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:08:52.883000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=8&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:53.714000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=8&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:08:56.941000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=5','151.38.137.85'),('2021-02-22 12:09:01.319000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=7&revTaskID=0&variante=5','151.38.137.85'),('2021-02-22 12:09:05.513000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:09:12.839000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=14&act=updatepos&posx=530&posy=311&variante=5','151.38.137.85'),('2021-02-22 12:09:13.925000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=8&revTaskID=0&variante=5','151.38.137.85'),('2021-02-22 12:09:17.540000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:09:17.934000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=8&revTaskID=0&variante=5','151.38.137.85'),('2021-02-22 12:09:18.386000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=14&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:09:19.023000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=14&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:09:20.043000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=14&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:09:21.054000','admin','Controller','/Products/Products/TaskParametersList','TaskID=14&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:09:22.178000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=14&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:09:23.065000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=14&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:09:24.174000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=14&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:09:24.612000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=14&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:09:30.569000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:09:39.742000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=9&act=updatepos&posx=772&posy=214&variante=5','151.38.137.85'),('2021-02-22 12:09:40.970000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=6&revTaskID=0&variante=5','151.38.137.85'),('2021-02-22 12:09:43.389000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:09:46.748000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=6&revTaskID=0&variante=5','151.38.137.85'),('2021-02-22 12:09:47.720000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=14&revTaskID=0&variante=5','151.38.137.85'),('2021-02-22 12:09:50.635000','admin','Page','/coldtech/Processi/pertManagePrecedenze2.aspx','id=14&revTaskID=0&variante=5','151.38.137.85'),('2021-02-22 12:09:52.056000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=9&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:09:52.719000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=9&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:09:53.722000','admin','Controller','/Products/Products/TaskParametersList','TaskID=9&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:09:56.524000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:09:57.261000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:09:58.092000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:09:58.954000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=9&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:09:59.784000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=9&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:10:00.810000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=9&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:10:03.191000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=9&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:10:03.808000','admin','Controller','/Products/Products/TaskParametersList','TaskID=9&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:10:05.332000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:10:06.227000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=9&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:10:08.498000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:10:09.346000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=9&TaskRev=0&variantID=5','151.38.137.85'),('2021-02-22 12:10:10.363000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=9&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:10:11.397000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=9&TaskRev=0&VariantID=5','151.38.137.85'),('2021-02-22 12:10:20.641000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=16&variante=5','151.38.137.85'),('2021-02-22 12:10:21.423000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=6&act=updatepos&posx=174&posy=156&variante=5','151.38.137.85'),('2021-02-22 12:11:41.158000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:11:53.464000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:11:56.295000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:11:58.015000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:02.567000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:04.399000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:12.402000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:13.935000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:17.590000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:19.223000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:23.544000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:25.013000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:37.011000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:12:48.519000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:13:00.155000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:13:11.687000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:13:23.164000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:13:34.696000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=16&variante=5&repID=1','151.38.137.85'),('2021-02-22 12:13:36.498000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.38.137.85'),('2021-02-22 12:15:05.948000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2','151.38.137.85'),('2021-02-22 12:15:13.695000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2','151.38.137.85'),('2021-02-22 12:15:29.468000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2','151.38.137.85'),('2021-02-22 12:15:31.550000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2','151.38.137.85'),('2021-02-22 12:15:37.574000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2','151.38.137.85'),('2021-02-22 12:15:39.696000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:15:41.095000','admin','Controller','/Products/ProductParameters/Index','processID=2&processRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:15:48.282000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:15:49.195000','admin','Controller','/Products/ProductParameters/Index','processID=2&processRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:16:00.686000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:16:13.017000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:16:16.809000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:16:18.795000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:16:20.639000','admin','Controller','/Products/ProductParameters/Index','processID=2&processRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:16:32.612000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:16:34.629000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=17&act=updatepos&posx=147&posy=178&variante=6','151.38.137.85'),('2021-02-22 12:16:35.075000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=17&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:16:35.994000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=17&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:16:37.024000','admin','Controller','/Products/Products/TaskParametersList','TaskID=17&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:16:38.096000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=17&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:16:39.048000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=17&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:16:40.147000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=17&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:16:44.628000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:16:45.283000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=17&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:16:54.400000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=17&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:16:56.884000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:16:57.348000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=17&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:01.905000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=17&act=updatepos&posx=221&posy=180&variante=6','151.38.137.85'),('2021-02-22 12:17:09.593000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:17:12.203000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=18&act=updatepos&posx=223&posy=348&variante=6','151.38.137.85'),('2021-02-22 12:17:12.648000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=18&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:13.264000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=18&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:14.286000','admin','Controller','/Products/Products/TaskParametersList','TaskID=18&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:15.814000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=18&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:17:16.852000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=18&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:17:17.959000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=18&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:17:21.929000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:17:25.947000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=18&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:32.068000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=18&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:34.542000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:17:35.338000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=18&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:44.756000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=19&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:45.395000','admin','Controller','/Products/Products/TaskParametersList','TaskID=19&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:46.938000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=19&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:17:47.955000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=19&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:17:48.972000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=19&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:50.050000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=19&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:50.598000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=19&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:17:53.263000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:17:55.624000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=19&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:17:56.456000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=19&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:18:05.746000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:18:07.048000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=19&act=updatepos&posx=213&posy=496&variante=6','151.38.137.85'),('2021-02-22 12:18:09.355000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=20&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:18:10.084000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=20&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:18:11.098000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=20&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:18:12.116000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=20&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:18:13.127000','admin','Controller','/Products/Products/TaskParametersList','TaskID=20&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:18:14.367000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=20&TaskRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:18:18.430000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:18:21.025000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=20&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:18:25.774000','admin','Action','/Products/Products/AddWorkingTimeToTask','TaskID=20&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:18:26.260000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=20&TaskRev=0&VariantID=6','151.38.137.85'),('2021-02-22 12:18:30.683000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:18:32.096000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=17&act=updatepos&posx=221&posy=207&variante=6','151.38.137.85'),('2021-02-22 12:18:33.370000','admin','Page','/coldtech/Processi/updatePERT.aspx','id=20&act=updatepos&posx=227&posy=82&variante=6','151.38.137.85'),('2021-02-22 12:19:22.171000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:19:27.044000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:19:28.633000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:19:32.276000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:19:33.756000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:19:37.361000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:19:38.776000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:19:42.290000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:19:43.886000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:19:56.378000','admin','Page','/coldtech/Reparti/managePostazioni.aspx','processID=2&variante=6&repID=1','151.38.137.85'),('2021-02-22 12:20:15.010000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:20:16.665000','admin','Controller','/Products/ProductParameters/Index','processID=2&processRev=0&variantID=6','151.38.137.85'),('2021-02-22 12:20:28.597000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:20:41.541000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:20:54.495000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:21:07.586000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:21:20.650000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:21:33.462000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:21:46.513000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:21:59.560000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:22:14.217000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:22:27.715000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:22:40.720000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:22:53.413000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:23:05.526000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:23:19.935000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:23:32.811000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:23:45.639000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:23:58.551000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:24:11.624000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:24:24.672000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:24:37.554000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:24:50.555000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:25:03.722000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:25:16.453000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:25:29.536000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:25:42.551000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:25:55.466000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:26:08.741000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:26:21.618000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:26:35.270000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:26:48.706000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:27:01.732000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:27:14.559000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:27:28.098000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:27:40.658000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:27:53.539000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:28:06.484000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:28:19.631000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:28:32.544000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:28:45.583000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:28:58.540000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:29:11.481000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:29:23.477000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:29:36.537000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:29:49.526000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:30:02.661000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:30:15.594000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:30:28.612000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:30:41.463000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:30:53.420000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:31:05.518000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:31:18.495000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:31:31.631000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:31:44.538000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:31:57.440000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:32:10.628000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:32:23.660000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:32:36.619000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:32:49.569000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:33:02.535000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:33:15.429000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:33:27.578000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:33:40.878000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:33:53.483000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:34:06.590000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:34:19.423000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:34:31.467000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:34:44.617000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:34:57.612000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:35:10.484000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:35:23.575000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:35:36.516000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:35:49.551000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:36:02.578000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:36:15.466000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:36:27.644000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:36:40.377000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:36:52.483000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:37:04.564000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:37:17.537000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:37:30.438000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.38.137.85'),('2021-02-22 12:37:43.329000','admin','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:37:50.642000','admin','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:37:50.769000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:37:53.522000','1r4gwqyq4523hvmoqf10jzq2','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:37:57.394000','1r4gwqyq4523hvmoqf10jzq2','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:38:02.724000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.38.137.85'),('2021-02-22 12:38:03.346000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.38.137.85'),('2021-02-22 12:38:06.506000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:38:06.634000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.38.137.85'),('2021-02-22 12:42:53.662000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:42:58.546000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:42:58.666000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.38.137.85'),('2021-02-22 12:43:50.745000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.38.137.85'),('2021-02-22 12:43:56.199000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=12','151.38.137.85'),('2021-02-22 12:45:34.577000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.38.137.85'),('2021-02-22 12:46:04.615000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.38.137.85'),('2021-02-22 12:46:15.036000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=13','151.38.137.85'),('2021-02-22 12:49:33.547000','yyyxgpmp3zx0hxiss2f2bcrp','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:49:39.561000','yyyxgpmp3zx0hxiss2f2bcrp','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 12:49:39.699000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.38.137.85'),('2021-02-22 13:30:23.996000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 13:30:28.029000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 13:30:28.343000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.38.137.85'),('2021-02-22 13:30:55.442000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 13:31:00.963000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 13:31:10.549000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.38.137.85'),('2021-02-22 13:31:13.955000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.38.137.85'),('2021-02-22 13:31:19.618000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.38.137.85'),('2021-02-22 13:32:45.272000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 13:32:51.777000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.38.137.85'),('2021-02-22 13:32:51.965000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.38.137.85'),('2021-02-22 13:33:31.516000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.38.137.85'),('2021-02-22 13:33:37.829000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=9','151.38.137.85'),('2021-02-22 14:06:24.570000','1r4gwqyq4523hvmoqf10jzq2','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.44.40.199'),('2021-02-22 14:06:26.656000','1r4gwqyq4523hvmoqf10jzq2','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 14:06:29.740000','1r4gwqyq4523hvmoqf10jzq2','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 14:06:36.018000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.44.40.199'),('2021-02-22 14:06:37.196000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.44.40.199'),('2021-02-22 14:06:45.114000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.44.40.199'),('2021-02-22 14:13:25.217000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.40.199'),('2021-02-22 14:13:36.725000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.40.199'),('2021-02-22 14:14:11.712000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.44.40.199'),('2021-02-22 14:14:37.738000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.44.40.199'),('2021-02-22 14:16:39.761000','produzione','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 14:24:54.198000','produzione','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 14:25:00.804000','produzione','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 14:25:00.913000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 14:25:05.073000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 14:25:05.416000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.40.199'),('2021-02-22 14:48:07.684000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 14:48:11.454000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 14:48:11.838000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.40.199'),('2021-02-22 15:01:27.393000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:01:31.084000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:01:31.198000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.44.40.199'),('2021-02-22 15:02:24.748000','spedizioni','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:25:48.870000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:25:52.702000','1h4n4aag32obgdlfnig4fwco','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:25:52.876000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.40.199'),('2021-02-22 15:26:19.457000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:26:22.488000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:26:25.573000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:26:25.686000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.44.40.199'),('2021-02-22 15:26:44.338000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.40.199'),('2021-02-22 15:29:33.355000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/HomePage/Default.aspx','','151.44.40.199'),('2021-02-22 15:29:36.500000','0x23sbjw3xnoa4woqwuc324k','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.44.40.199'),('2021-02-22 15:29:36.747000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:29:42.659000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:29:47.988000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.44.40.199'),('2021-02-22 15:29:49.068000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=24/01/2021&endPeriod=24/02/2021&periodType=1','151.44.40.199'),('2021-02-22 15:30:01.944000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.44.40.199'),('2021-02-22 15:31:56.377000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.44.40.199'),('2021-02-22 15:31:59.891000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=11','151.44.40.199'),('2021-02-22 15:32:44.504000','1r4gwqyq4523hvmoqf10jzq2','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:32:51.201000','1r4gwqyq4523hvmoqf10jzq2','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:32:51.372000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.40.199'),('2021-02-22 15:34:17.241000','produzione','Page','/coldtech/Login/login.aspx','','151.44.40.199'),('2021-02-22 15:34:21.766000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.40.199'),('2021-02-22 15:54:13.651000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.44.40.199'),('2021-02-22 16:15:02.243000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.44.40.199'),('2021-02-22 16:15:18.544000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=F','151.44.40.199'),('2021-02-22 16:47:39.340000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=F','151.44.40.199'),('2021-02-23 07:20:23.297000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/HomePage/Default.aspx','','151.36.180.4'),('2021-02-23 07:20:25.328000','lh10h54mdlklfootn4hsyljd','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.36.180.4'),('2021-02-23 07:20:38.843000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:20:41.328000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/HomePage/Default.aspx','','151.36.180.4'),('2021-02-23 07:20:41.921000','q2m2xtnsjc3tayuaxuxydpyp','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.36.180.4'),('2021-02-23 07:20:51.078000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:20:53.500000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:20:55.734000','admin','Page','/coldtech/HomePage/Default.aspx','','151.36.180.4'),('2021-02-23 07:20:58.140000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.36.180.4'),('2021-02-23 07:20:58.843000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:21:03.281000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.36.180.4'),('2021-02-23 07:21:05.093000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.36.180.4'),('2021-02-23 07:21:27.093000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.180.4'),('2021-02-23 07:21:30.406000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.36.180.4'),('2021-02-23 07:21:41.391000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3','151.36.180.4'),('2021-02-23 07:22:11.968000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:22:52.014000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:22:57.936000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:22:58.030000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.180.4'),('2021-02-23 07:23:21.639000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:23:31.092000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:23:31.186000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:24:16.263000','timeadmin','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:24:17.685000','timeadmin','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:24:17.779000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:24:30.388000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:24:37.232000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:24:45.935000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:25:16.372000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:25:20.716000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:25:20.794000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.180.4'),('2021-02-23 07:25:27.903000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:25:30.013000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:25:30.091000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:27:48.798000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:27:48.901000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.180.4'),('2021-02-23 07:27:58.235000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:28:00.553000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:28:00.646000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:28:12.473000','q2m2xtnsjc3tayuaxuxydpyp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:28:17.536000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.36.180.4'),('2021-02-23 07:28:18.261000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.36.180.4'),('2021-02-23 07:28:52.374000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:28:57.189000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:28:57.317000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.180.4'),('2021-02-23 07:29:46.195000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.36.180.4'),('2021-02-23 07:29:50.887000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=3','151.36.180.4'),('2021-02-23 07:32:22.866000','hlpw1fh5tdbpsljve3m3up2j','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:34:57.325000','nb3jzft0ekaiey3cutyihi3z','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:34:57.435000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.180.4'),('2021-02-23 07:36:37.419000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.180.4'),('2021-02-23 07:36:41.050000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.180.4'),('2021-02-23 07:39:14.829000','h2j3d44ekcg1krt2ng1jbebp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:39:20.746000','h2j3d44ekcg1krt2ng1jbebp','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:39:20.875000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.180.4'),('2021-02-23 07:40:22.776000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.180.4'),('2021-02-23 07:40:27.019000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=15','151.36.180.4'),('2021-02-23 07:41:03.370000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.36.180.4'),('2021-02-23 07:41:04.630000','timeadmin','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:41:13.166000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=F','151.36.180.4'),('2021-02-23 07:59:55.196000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 07:59:55.551000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 08:01:01.959000','q2m2xtnsjc3tayuaxuxydpyp','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=F','151.36.180.4'),('2021-02-23 08:01:07.660000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 08:01:07.930000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.180.4'),('2021-02-23 08:07:58.109000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 08:08:02.489000','vrh14ga4tpiqbdfi3lvomiza','Page','/coldtech/Login/login.aspx','','151.36.180.4'),('2021-02-23 08:08:02.979000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.180.4'),('2021-02-23 12:52:23.393000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/HomePage/Default.aspx','','151.36.206.199'),('2021-02-23 12:52:26.471000','lh10h54mdlklfootn4hsyljd','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.36.206.199'),('2021-02-23 12:52:31.080000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 12:52:34.752000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 12:52:38.971000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.36.206.199'),('2021-02-23 12:52:40.830000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.36.206.199'),('2021-02-23 12:52:52.174000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.36.206.199'),('2021-02-23 12:53:09.705000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.206.199'),('2021-02-23 12:53:14.174000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.36.206.199'),('2021-02-23 12:53:14.909000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.36.206.199'),('2021-02-23 12:53:22.768000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.206.199'),('2021-02-23 12:55:50.690000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=05/03/2021 00:00:00&plannedenddate=12/03/2021 00:00:00&DepartmentId=1&name=Ante lotto 1&description=&processid=16&processrev=0&variantid=5&serialnumber=&quantity=6&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.36.206.199'),('2021-02-23 12:55:51.643000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.206.199'),('2021-02-23 12:55:52.831000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=05/03/2021 00:00:00&plannedenddate=12/03/2021 00:00:00&DepartmentId=1&name=Ante lotto 1&description=&processid=16&processrev=0&variantid=5&serialnumber=&quantity=6&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.36.206.199'),('2021-02-23 12:57:54.519000','xg1skaqkysteakyysonnmnrl','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 12:58:11.722000','xg1skaqkysteakyysonnmnrl','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 12:58:11.847000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.206.199'),('2021-02-23 13:08:18.503000','g2m4a2vcwmbhca1l5gzxpehh','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 13:08:27.941000','g2m4a2vcwmbhca1l5gzxpehh','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 13:08:28.066000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.206.199'),('2021-02-23 13:09:33.347000','timeadmin','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 13:09:35.644000','timeadmin','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 13:09:35.738000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 13:09:44.457000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.206.199'),('2021-02-23 13:09:48.691000','admin','Page','/coldtech/HomePage/Default.aspx','','151.36.206.199'),('2021-02-23 13:09:49.738000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.36.206.199'),('2021-02-23 13:10:10.207000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:10:27.800000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:10:29.144000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:10:35.394000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.206.199'),('2021-02-23 13:10:37.379000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.206.199'),('2021-02-23 13:10:48.863000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:10:50.488000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:10:54.175000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.206.199'),('2021-02-23 13:10:58.300000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.206.199'),('2021-02-23 13:11:31.691000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:11:33.097000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:11:37.394000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.206.199'),('2021-02-23 13:11:39.019000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.206.199'),('2021-02-23 13:16:12.395000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:16:14.004000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:16:16.160000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.206.199'),('2021-02-23 13:17:12.035000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:17:13.395000','admin','Page','/coldtech/Admin/kisAdmin.aspx','','151.36.206.199'),('2021-02-23 13:17:18.285000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.206.199'),('2021-02-23 13:17:20.832000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.206.199'),('2021-02-23 13:19:19.567000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.206.199'),('2021-02-23 14:52:05.630000','vwpj100cecmg2hbzr5mriw5t','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-23 14:56:19.037000','gzdcxxgby3gzm43fi5uaqykh','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','188.12.146.140'),('2021-02-23 14:56:31.365000','gzdcxxgby3gzm43fi5uaqykh','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','188.12.146.140'),('2021-02-23 14:56:32.052000','gzdcxxgby3gzm43fi5uaqykh','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-23 14:56:38.146000','vwpj100cecmg2hbzr5mriw5t','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-23 14:56:38.256000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','188.12.146.140'),('2021-02-23 14:56:52.990000','gzdcxxgby3gzm43fi5uaqykh','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-23 14:56:53.084000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','188.12.146.140'),('2021-02-23 15:05:34.927000','produzione','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-23 15:05:35.599000','produzione','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-23 15:57:44.240000','gzdcxxgby3gzm43fi5uaqykh','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-23 21:11:15.120000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/HomePage/Default.aspx','','151.18.4.226'),('2021-02-23 21:11:17.729000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:11:17.807000','lh10h54mdlklfootn4hsyljd','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.18.4.226'),('2021-02-23 21:11:20.917000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:11:25.213000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.18.4.226'),('2021-02-23 21:11:27.854000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.18.4.226'),('2021-02-23 21:11:31.010000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.18.4.226'),('2021-02-23 21:11:31.948000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.18.4.226'),('2021-02-23 21:11:48.260000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.18.4.226'),('2021-02-23 21:12:05.932000','timeadmin','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:12:07.948000','timeadmin','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:12:08.042000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:12:11.448000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:12:11.557000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.18.4.226'),('2021-02-23 21:22:37.621000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.18.4.226'),('2021-02-23 21:22:42.558000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.18.4.226'),('2021-02-23 21:24:30.636000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.18.4.226'),('2021-02-23 21:24:59.699000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.18.4.226'),('2021-02-23 21:25:02.808000','produzione','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:25:07.777000','produzione','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:25:07.902000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:25:13.308000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.18.4.226'),('2021-02-23 21:25:17.605000','admin','Page','/coldtech/HomePage/Default.aspx','','151.18.4.226'),('2021-02-23 21:25:19.183000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=25/01/2021&endPeriod=25/02/2021&periodType=1','151.18.4.226'),('2021-02-23 21:26:08.761000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.18.4.226'),('2021-02-23 21:26:52.824000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.18.4.226'),('2021-02-23 21:30:06.686000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.18.4.226'),('2021-02-23 21:41:09.104000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=16','151.18.4.226'),('2021-02-23 21:41:34.034000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.18.4.226'),('2021-02-23 22:05:55.675000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.152.103'),('2021-02-23 22:06:06.487000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.152.103'),('2021-02-23 22:06:07.949000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.152.103'),('2021-02-24 06:26:30.986000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/HomePage/Default.aspx','','151.68.182.221'),('2021-02-24 06:26:35.267000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 06:26:36.017000','0x23sbjw3xnoa4woqwuc324k','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.68.182.221'),('2021-02-24 06:26:38.392000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 06:26:38.501000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.182.221'),('2021-02-24 06:31:00.642000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.68.182.221'),('2021-02-24 06:31:33.142000','p40cgbexwkykownkyh3nv1rv','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 06:31:35.173000','p40cgbexwkykownkyh3nv1rv','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 06:31:39.627000','p40cgbexwkykownkyh3nv1rv','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 06:31:44.064000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.68.182.221'),('2021-02-24 06:31:46.002000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.68.182.221'),('2021-02-24 06:31:47.939000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 06:32:00.455000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=14','151.68.182.221'),('2021-02-24 06:42:03.089000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=18','151.68.182.221'),('2021-02-24 06:42:22.073000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.182.221'),('2021-02-24 06:42:32.804000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=18','151.68.182.221'),('2021-02-24 07:03:50.665000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 07:03:51.055000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 07:06:02.047000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 07:06:02.337000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.182.221'),('2021-02-24 07:10:51.405000','p40cgbexwkykownkyh3nv1rv','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=14','151.68.182.221'),('2021-02-24 07:10:57.563000','p40cgbexwkykownkyh3nv1rv','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 07:11:00.228000','p40cgbexwkykownkyh3nv1rv','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 07:11:03.492000','p40cgbexwkykownkyh3nv1rv','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 07:11:11.676000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.68.182.221'),('2021-02-24 07:11:15.812000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.68.182.221'),('2021-02-24 07:11:17.558000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 07:11:53.272000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.182.221'),('2021-02-24 07:12:15.030000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=24/02/2021 00:00:00&plannedenddate=27/02/2021 00:00:00&DepartmentId=1&name=End&description=&processid=0&processrev=0&variantid=4&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.68.182.221'),('2021-02-24 07:12:16.628000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 07:12:50.150000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 07:13:16.426000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 07:13:18.763000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=17','151.68.182.221'),('2021-02-24 07:16:31.940000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 07:16:36.679000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=18','151.68.182.221'),('2021-02-24 07:30:48.738000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.182.221'),('2021-02-24 08:20:49.092000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/HomePage/Default.aspx','','151.68.182.221'),('2021-02-24 08:20:53.812000','0x23sbjw3xnoa4woqwuc324k','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.68.182.221'),('2021-02-24 08:21:46.225000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 08:21:50.855000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 08:21:55.452000','admin','Page','/coldtech/HomePage/Default.aspx','','151.68.182.221'),('2021-02-24 08:21:56.266000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.68.182.221'),('2021-02-24 08:22:06.350000','lh10h54mdlklfootn4hsyljd','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 08:22:16.744000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 08:22:19.685000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 08:22:24.321000','admin','Page','/coldtech/HomePage/Default.aspx','','151.68.182.221'),('2021-02-24 08:22:25.011000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.68.182.221'),('2021-02-24 08:22:26.937000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 08:23:32.337000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=01/03/2021 00:00:00&plannedenddate=07/03/2021 00:00:00&DepartmentId=1&name=Ante 1&description=&processid=16&processrev=0&variantid=5&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.68.182.221'),('2021-02-24 08:23:33.245000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 08:23:53.836000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.182.221'),('2021-02-24 08:57:11.543000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:13:34.376000','admin','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:13:38.305000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:13:57.695000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=01/03/2021 00:00:00&plannedenddate=07/03/2021 00:00:00&DepartmentId=1&name=Maniglia 1&description=&processid=2&processrev=0&variantid=6&serialnumber=&quantity=100&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.68.182.221'),('2021-02-24 09:13:58.562000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:14:21.040000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=24/02/2021 00:00:00&plannedenddate=28/02/2021 00:00:00&DepartmentId=2&name=Spedizioni W8&description=&processid=3&processrev=0&variantid=2&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.68.182.221'),('2021-02-24 09:14:22.012000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:15:13.988000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=24/02/2021 00:00:00&plannedenddate=28/02/2021 00:00:00&DepartmentId=1&name=Kit porte 1&description=&processid=0&processrev=0&variantid=4&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.68.182.221'),('2021-02-24 09:15:14.884000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:15:32.665000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=24/02/2021 00:00:00&plannedenddate=28/02/2021 00:00:00&DepartmentId=1&name=Porte 1&description=&processid=1&processrev=0&variantid=3&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.68.182.221'),('2021-02-24 09:15:33.708000','admin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:17:59.444000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/HomePage/Default.aspx','','151.68.182.221'),('2021-02-24 09:18:03.648000','0x23sbjw3xnoa4woqwuc324k','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.68.182.221'),('2021-02-24 09:18:07.448000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:18:13.479000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:18:13.613000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.182.221'),('2021-02-24 09:19:35.291000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.68.182.221'),('2021-02-24 09:20:17.319000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:20:21.739000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:20:26.252000','admin','Page','/coldtech/HomePage/Default.aspx','','151.68.182.221'),('2021-02-24 09:20:26.953000','admin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.68.182.221'),('2021-02-24 09:20:28.061000','admin','Page','/coldtech/Processi/MacroProcessi.aspx','','151.68.182.221'),('2021-02-24 09:20:51.780000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2','151.68.182.221'),('2021-02-24 09:20:58.862000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:21:00.162000','admin','Controller','/Products/ProductParameters/Index','processID=2&processRev=0&variantID=6','151.68.182.221'),('2021-02-24 09:21:02.933000','admin','Controller','/Products/Products/EditTaskPanel','TaskID=20&TaskRev=0&VariantID=6','151.68.182.221'),('2021-02-24 09:21:04.612000','admin','Controller','/Products/Products/TaskCycleTimesList','TaskID=20&TaskRev=0&VariantID=6','151.68.182.221'),('2021-02-24 09:21:05.626000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=20&TaskRev=0&variantID=6','151.68.182.221'),('2021-02-24 09:21:06.639000','admin','Controller','/Products/Products/TaskParametersList','TaskID=20&TaskRev=0&VariantID=6','151.68.182.221'),('2021-02-24 09:21:08.167000','admin','Controller','/Products/Products/TaskDefaultOperatorsList','TaskID=20&TaskRev=0&variantID=6','151.68.182.221'),('2021-02-24 09:21:09.203000','admin','Controller','/Products/Products/TaskMicrostepsList','TaskID=20&TaskRev=0&variantID=6','151.68.182.221'),('2021-02-24 09:21:11.498000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:21:15.562000','admin','Action','/Products/Products/SaveTaskDetails','TaskID=20&TaskRev=0&VariantID=6','151.68.182.221'),('2021-02-24 09:21:23.607000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:21:35.482000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:21:47.752000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:21:59.679000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:22:11.517000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:22:23.552000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:22:35.468000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:22:47.389000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:22:59.259000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:23:11.040000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:23:22.930000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:23:34.769000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:23:47.102000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:23:59.023000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:24:10.843000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:24:22.680000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:24:34.585000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:24:46.453000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:24:58.187000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:25:10.074000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:25:22.290000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:25:34.291000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:25:46.138000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:25:58.489000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:26:03.411000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.182.221'),('2021-02-24 09:26:11.398000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:26:23.312000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:26:35.271000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:26:47.467000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:27:00.301000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:27:12.205000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:27:24.184000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:27:36.209000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:27:48.240000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:28:00.293000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:28:12.201000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:28:24.276000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:28:36.267000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:28:48.178000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:29:00.186000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:29:12.338000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:29:24.281000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:29:36.291000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:29:48.313000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:30:01.721000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:30:14.293000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:30:26.391000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:30:38.629000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:30:51.163000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:31:03.288000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:31:15.234000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:31:27.213000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:31:39.338000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:31:51.418000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:31:58.087000','admin','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:32:03.577000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:32:16.176000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:32:28.368000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:32:40.570000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:32:53.290000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:33:05.492000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:33:18.363000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:33:31.359000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:33:43.299000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:33:55.273000','admin','Page','/coldtech/Processi/showProcesso.aspx','id=2&variante=6','151.68.182.221'),('2021-02-24 09:34:01.434000','admin','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:34:05.017000','admin','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:34:05.121000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:34:08.723000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:34:13.520000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.68.182.221'),('2021-02-24 09:34:15.749000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.68.182.221'),('2021-02-24 09:38:16.812000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:49:42.088000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:49:59.507000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=01/03/2021 00:00:00&plannedenddate=07/03/2021 00:00:00&DepartmentId=1&name=Ante 3&description=&processid=16&processrev=0&variantid=5&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=False&AllowExecuteFinishedTasks=False','151.68.182.221'),('2021-02-24 09:50:00.389000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:50:52.442000','qkcpvtn102ze021njb0zvcxc','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.68.182.221'),('2021-02-24 09:51:01.945000','qkcpvtn102ze021njb0zvcxc','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:51:05.098000','qkcpvtn102ze021njb0zvcxc','Page','/coldtech/Login/login.aspx','','151.68.182.221'),('2021-02-24 09:51:05.195000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.68.182.221'),('2021-02-24 09:52:41.401000','gzdcxxgby3gzm43fi5uaqykh','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-24 09:52:58.136000','gzdcxxgby3gzm43fi5uaqykh','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-24 09:52:58.285000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','188.12.146.140'),('2021-02-24 10:39:28.281000','gzdcxxgby3gzm43fi5uaqykh','Page','/coldtech/Login/login.aspx','','188.12.146.140'),('2021-02-24 11:15:40.452000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.38.29.128'),('2021-02-24 11:44:28.371000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.38.29.128'),('2021-02-24 11:49:23.105000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.38.29.128'),('2021-02-24 14:32:44.140000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.36.176.83'),('2021-02-24 14:37:57.094000','timeadmin','Page','/coldtech/Config/NoProductiveTasks/Index','','151.36.176.83'),('2021-02-24 14:59:50.048000','timeadmin','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 14:59:53.579000','timeadmin','Page','/coldtech/Personal/my.aspx','','151.36.176.83'),('2021-02-24 15:00:06.485000','timeadmin','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:00:06.579000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:00:53.766000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:00:53.985000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.176.83'),('2021-02-24 15:00:57.157000','produzione','Page','/coldtech/Personal/my.aspx','','151.36.176.83'),('2021-02-24 15:01:00.282000','produzione','Controller','/Personal/PersonalArea/EditDestinationURL','','151.36.176.83'),('2021-02-24 15:01:02.563000','produzione','Page','/coldtech/Personal/my.aspx','','151.36.176.83'),('2021-02-24 15:01:12.407000','produzione','Page','/coldtech/Personal/my.aspx','','151.36.176.83'),('2021-02-24 15:01:14.829000','produzione','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:01:16.735000','produzione','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:01:16.813000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:01:19.485000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:01:19.641000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.176.83'),('2021-02-24 15:01:21.876000','spedizioni','Page','/coldtech/Personal/my.aspx','','151.36.176.83'),('2021-02-24 15:01:24.766000','spedizioni','Controller','/Personal/PersonalArea/EditDestinationURL','','151.36.176.83'),('2021-02-24 15:01:27.985000','spedizioni','Page','/coldtech/Personal/my.aspx','','151.36.176.83'),('2021-02-24 15:01:35.376000','spedizioni','Page','/coldtech/Personal/my.aspx','','151.36.176.83'),('2021-02-24 15:01:39.079000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:01:40.954000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:01:41.032000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:01:45.173000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:01:49.595000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.36.176.83'),('2021-02-24 15:01:51.970000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.36.176.83'),('2021-02-24 15:02:03.860000','timeadmin','Page','/coldtech/Personal/my.aspx','','151.36.176.83'),('2021-02-24 15:02:07.016000','timeadmin','Controller','/Personal/PersonalArea/EditDestinationURL','','151.36.176.83'),('2021-02-24 15:02:13.048000','timeadmin','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:02:14.845000','timeadmin','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:02:14.938000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:02:17.688000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:02:17.782000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.176.83'),('2021-02-24 15:02:20.876000','produzione','Page','/coldtech/Personal/my.aspx','','151.36.176.83'),('2021-02-24 15:02:23.579000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.176.83'),('2021-02-24 15:02:24.501000','produzione','Controller','/Personal/PersonalArea/EditDestinationURL','','151.36.176.83'),('2021-02-24 15:24:49.560000','j3unulnlx1uhzbaoun1uocgs','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:24:49.685000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.176.83'),('2021-02-24 15:25:01.966000','spedizioni','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.176.83'),('2021-02-24 15:27:03.076000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:27:05.576000','spedizioni','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:27:05.654000','j3unulnlx1uhzbaoun1uocgs','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:29:29.545000','lsllkdbhietv2elgdwazqaga','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:29:35.092000','lsllkdbhietv2elgdwazqaga','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:29:40.607000','lsllkdbhietv2elgdwazqaga','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:29:44.732000','lsllkdbhietv2elgdwazqaga','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 15:29:44.857000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.176.83'),('2021-02-24 15:36:48.419000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.176.83'),('2021-02-24 15:41:40.310000','hrapzhhxpjghqgmunhuxt1t3','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 16:48:53.657000','utrfd530sur0ytjio4hvcnan','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 16:48:53.845000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.176.83'),('2021-02-24 16:52:42.393000','ixzltivhxxhke3skqzcglfrr','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 17:05:32.145000','lh10h54mdlklfootn4hsyljd','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.176.83'),('2021-02-24 17:05:33.801000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 17:05:36.333000','lh10h54mdlklfootn4hsyljd','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 17:05:36.505000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.176.83'),('2021-02-24 17:34:14.476000','jgpcfynpb2itdbwzbnptke0u','Page','/coldtech/HomePage/Default.aspx','','151.36.176.83'),('2021-02-24 17:34:14.992000','jgpcfynpb2itdbwzbnptke0u','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.36.176.83'),('2021-02-24 17:34:16.429000','jgpcfynpb2itdbwzbnptke0u','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 17:34:19.367000','jgpcfynpb2itdbwzbnptke0u','Page','/coldtech/Login/login.aspx','','151.36.176.83'),('2021-02-24 17:34:23.492000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.36.176.83'),('2021-02-24 17:34:25.273000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=26/01/2021&endPeriod=26/02/2021&periodType=1','151.36.176.83'),('2021-02-24 17:34:27.085000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.176.83'),('2021-02-24 17:34:37.664000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=23','151.36.176.83'),('2021-02-24 17:35:39.617000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.36.176.83'),('2021-02-24 17:35:49.414000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.36.176.83'),('2021-02-24 17:35:59.773000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.36.176.83'),('2021-02-24 17:36:02.804000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=20','151.36.176.83'),('2021-02-25 12:56:39.741000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/HomePage/Default.aspx','','151.44.54.211'),('2021-02-25 12:56:42.569000','0x23sbjw3xnoa4woqwuc324k','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=27/01/2021&endPeriod=27/02/2021&periodType=1','151.44.54.211'),('2021-02-25 12:56:47.663000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.44.54.211'),('2021-02-25 12:57:14.241000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.44.54.211'),('2021-02-25 12:57:14.366000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.54.211'),('2021-02-25 13:01:28.522000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.44.54.211'),('2021-02-25 13:03:12.226000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.54.211'),('2021-02-25 13:03:47.929000','m3awqzd0rhxngt44wxos4r1o','Page','/coldtech/HomePage/Default.aspx','','151.44.54.211'),('2021-02-25 13:03:48.585000','m3awqzd0rhxngt44wxos4r1o','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=27/01/2021&endPeriod=27/02/2021&periodType=1','151.44.54.211'),('2021-02-25 13:04:51.710000','m3awqzd0rhxngt44wxos4r1o','Page','/coldtech/Login/login.aspx','','151.44.54.211'),('2021-02-25 13:04:55.632000','m3awqzd0rhxngt44wxos4r1o','Page','/coldtech/Login/login.aspx','','151.44.54.211'),('2021-02-25 13:04:59.898000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.44.54.211'),('2021-02-25 13:05:01.070000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=27/01/2021&endPeriod=27/02/2021&periodType=1','151.44.54.211'),('2021-02-25 13:05:04.616000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.44.54.211'),('2021-02-25 13:08:27.991000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.44.54.211'),('2021-02-25 13:16:14.295000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.44.54.211'),('2021-02-25 13:16:19.382000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=21','151.44.54.211'),('2021-02-25 15:14:52.867000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.82.94.20'),('2021-02-25 15:14:55.226000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.82.94.20'),('2021-02-25 15:14:55.336000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.82.94.20'),('2021-02-25 15:17:49.101000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=2','151.82.94.20'),('2021-02-25 20:43:16.565000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/HomePage/Default.aspx','','151.46.10.200'),('2021-02-25 20:43:17.815000','0x23sbjw3xnoa4woqwuc324k','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=27/01/2021&endPeriod=27/02/2021&periodType=1','151.46.10.200'),('2021-02-25 20:43:23.034000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.46.10.200'),('2021-02-25 20:43:33.956000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.46.10.200'),('2021-02-25 20:43:35.128000','produzione','Controller','/FreeTimeMeasurement/FreeMeasurement/Execute','DepartmentId=1','151.46.10.200'),('2021-02-25 20:43:45.706000','mlgcj2cnhajr1tbtzb4xlido','Page','/coldtech/HomePage/Default.aspx','','151.46.10.200'),('2021-02-25 20:43:46.393000','mlgcj2cnhajr1tbtzb4xlido','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=27/01/2021&endPeriod=27/02/2021&periodType=1','151.46.10.200'),('2021-02-25 20:43:50.659000','mlgcj2cnhajr1tbtzb4xlido','Page','/coldtech/Login/login.aspx','','151.46.10.200'),('2021-02-25 20:43:55.268000','mlgcj2cnhajr1tbtzb4xlido','Page','/coldtech/Login/login.aspx','','151.46.10.200'),('2021-02-25 20:44:00.159000','timeadmin','Page','/coldtech/HomePage/Default.aspx','','151.46.10.200'),('2021-02-25 20:44:01.284000','timeadmin','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=27/01/2021&endPeriod=27/02/2021&periodType=1','151.46.10.200'),('2021-02-25 20:44:03.706000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.46.10.200'),('2021-02-25 20:49:14.863000','produzione','Page','/coldtech/Login/login.aspx','','151.46.10.200'),('2021-02-25 20:49:16.722000','produzione','Page','/coldtech/Login/login.aspx','','151.46.10.200'),('2021-02-25 20:49:16.816000','0x23sbjw3xnoa4woqwuc324k','Page','/coldtech/Login/login.aspx','','151.46.10.200'),('2021-02-25 20:49:25.034000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.46.10.200'),('2021-02-25 20:49:39.284000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=24','151.46.10.200'),('2021-02-25 20:50:40.019000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Finish','MeasurementId=22','151.46.10.200'),('2021-02-25 20:51:06.628000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=01/03/2021 00:00:00&plannedenddate=07/03/2021 00:00:00&DepartmentId=2&name=W9&description=&processid=3&processrev=0&variantid=2&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.46.10.200'),('2021-02-25 20:51:07.691000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.46.10.200'),('2021-02-25 20:51:33.738000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=01/03/2021 00:00:00&plannedenddate=07/03/2021 00:00:00&DepartmentId=1&name=Maniglie 2&description=&processid=2&processrev=0&variantid=6&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=False','151.46.10.200'),('2021-02-25 20:51:34.878000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.46.10.200'),('2021-02-25 20:51:53.988000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=01/03/2021 00:00:00&plannedenddate=07/03/2021 00:00:00&DepartmentId=1&name=Kit porte 2&description=Test&processid=0&processrev=0&variantid=4&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.46.10.200'),('2021-02-25 20:51:55.284000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.46.10.200'),('2021-02-25 20:52:22.160000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Add','plannedstartdate=01/03/2021 00:00:00&plannedenddate=07/03/2021 00:00:00&DepartmentId=1&name=Porte 2&description=&processid=1&processrev=0&variantid=3&serialnumber=&quantity=1&measurementUnitId=0&AllowCustomTasks=True&AllowExecuteFinishedTasks=True','151.46.10.200'),('2021-02-25 20:52:23.456000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=O','151.46.10.200'),('2021-02-25 20:52:36.972000','timeadmin','Controller','/FreeTimeMeasurement/FreeMeasurement/Index','Status=F','151.46.10.200'),('2021-02-26 10:22:36.901000','lh10h54mdlklfootn4hsyljd','Page','/telwin/HomePage/Default.aspx','','151.18.4.95'),('2021-02-26 10:22:41.185000','lh10h54mdlklfootn4hsyljd','Controller','/Analysis/GlobalKPIsController/GetGlobalKPIs','startPeriod=28/01/2021&endPeriod=28/02/2021&periodType=1','151.18.4.95');
/*!40000 ALTER TABLE `userslog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `varianti`
--

DROP TABLE IF EXISTS `varianti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `varianti` (
  `idvariante` int(11) NOT NULL,
  `nomeVariante` varchar(255) NOT NULL,
  `descVariante` text,
  PRIMARY KEY (`idvariante`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `varianti`
--

LOCK TABLES `varianti` WRITE;
/*!40000 ALTER TABLE `varianti` DISABLE KEYS */;
INSERT INTO `varianti` VALUES (0,'Test','Test'),(1,'Test2','Test2'),(2,'Spedizione','New Default Version'),(3,'Porta',''),(4,'Kit porte','Kit porte'),(5,'Anta','Anta'),(6,'Accessori maniglie','Accessori maniglie'),(7,'New Product','AAAAA');
/*!40000 ALTER TABLE `varianti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `variantiprocessi`
--

DROP TABLE IF EXISTS `variantiprocessi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `variantiprocessi` (
  `variante` int(11) NOT NULL,
  `processo` int(11) NOT NULL,
  `revProc` int(11) NOT NULL,
  `ExternalID` varchar(255) DEFAULT NULL,
  `measurementUnit` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`variante`,`processo`,`revProc`),
  UNIQUE KEY `ExternalID_UNIQUE` (`ExternalID`),
  KEY `variante_FK` (`variante`),
  KEY `processo_FK` (`processo`),
  KEY `variantiprocessi_processo_FK` (`processo`,`revProc`),
  KEY `measurementunit_FK1_idx` (`measurementUnit`),
  CONSTRAINT `measurementunit_FK1` FOREIGN KEY (`measurementUnit`) REFERENCES `measurementunits` (`id`),
  CONSTRAINT `variante_FK` FOREIGN KEY (`variante`) REFERENCES `varianti` (`idvariante`),
  CONSTRAINT `variantiprocessi_processo_FK` FOREIGN KEY (`processo`, `revProc`) REFERENCES `processo` (`processid`, `revisione`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `variantiprocessi`
--

LOCK TABLES `variantiprocessi` WRITE;
/*!40000 ALTER TABLE `variantiprocessi` DISABLE KEYS */;
INSERT INTO `variantiprocessi` VALUES (2,3,0,'SPED_STD',0),(3,1,0,'PORTA',0),(4,0,0,'KIT_PORTE_STD',0),(5,16,0,'ANTA_STD',0),(6,2,0,'ACCESSORI_STD',0),(7,1,0,'123',0);
/*!40000 ALTER TABLE `variantiprocessi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `warningproduzione`
--

DROP TABLE IF EXISTS `warningproduzione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `warningproduzione` (
  `id` int(11) NOT NULL,
  `dataChiamata` datetime NOT NULL,
  `task` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  `dataRisoluzione` datetime DEFAULT NULL,
  `motivo` text,
  `risoluzione` text,
  PRIMARY KEY (`id`),
  KEY `warning_FK1` (`task`),
  KEY `warning_FK2` (`user`),
  CONSTRAINT `warning_FK1` FOREIGN KEY (`task`) REFERENCES `tasksproduzione` (`taskid`),
  CONSTRAINT `warning_FK2` FOREIGN KEY (`user`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `warningproduzione`
--

LOCK TABLES `warningproduzione` WRITE;
/*!40000 ALTER TABLE `warningproduzione` DISABLE KEYS */;
/*!40000 ALTER TABLE `warningproduzione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `workinstructionslabel`
--

DROP TABLE IF EXISTS `workinstructionslabel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `workinstructionslabel` (
  `wilabelID` int(11) NOT NULL,
  `wilabelName` varchar(255) NOT NULL,
  PRIMARY KEY (`wilabelID`),
  UNIQUE KEY `wilabelName_UNIQUE` (`wilabelName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workinstructionslabel`
--

LOCK TABLES `workinstructionslabel` WRITE;
/*!40000 ALTER TABLE `workinstructionslabel` DISABLE KEYS */;
/*!40000 ALTER TABLE `workinstructionslabel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Final view structure for view `freemeasurements_errors_in_tasks`
--

/*!50001 DROP VIEW IF EXISTS `freemeasurements_errors_in_tasks`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `freemeasurements_errors_in_tasks` AS select `freemeasurements_tasks`.`MeasurementId` AS `MeasurementId`,`freemeasurements_tasks`.`TaskId` AS `TaskId`,`freemeasurements_tasks`.`OrigTaskId` AS `OrigTaskId`,`freemeasurements_tasks`.`OrigTaskRev` AS `OrigTaskRev`,`freemeasurements_tasks`.`VariantId` AS `VariantId`,`freemeasurements_tasks`.`NoProductiveTaskId` AS `NoProductiveTaskId`,`freemeasurements_tasks`.`name` AS `name`,`freemeasurements_tasks`.`description` AS `description`,`freemeasurements_tasks`.`sequence` AS `sequence`,`freemeasurements_tasks`.`workstationid` AS `workstationid`,`freemeasurements_tasks`.`quantity_planned` AS `quantity_planned`,`freemeasurements_tasks`.`quantity_produced` AS `quantity_produced`,`freemeasurements_tasks`.`status` AS `status`,`freemeasurements_tasks`.`task_startdatereal` AS `task_startdatereal`,`freemeasurements_tasks`.`task_enddatereal` AS `task_enddatereal`,`freemeasurements_tasks`.`realleadtime_hours` AS `realleadtime_hours`,`freemeasurements_tasks`.`realworkingtime_hours` AS `realworkingtime_hours` from `freemeasurements_tasks` where ((`freemeasurements_tasks`.`status` = 'F') and ((isnull(`freemeasurements_tasks`.`realleadtime_hours`) xor isnull(`freemeasurements_tasks`.`realworkingtime_hours`)) or isnull(`freemeasurements_tasks`.`task_enddatereal`) or isnull(`freemeasurements_tasks`.`task_startdatereal`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `freemeasurements_events_full_view`
--

/*!50001 DROP VIEW IF EXISTS `freemeasurements_events_full_view`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `freemeasurements_events_full_view` AS select `freemeasurements_tasks_events`.`id` AS `id`,`freemeasurements_tasks_events`.`user` AS `user`,`freemeasurements_tasks_events`.`eventtype` AS `eventtype`,`freemeasurements_tasks_events`.`eventdate` AS `eventdate`,`freemeasurements_tasks_events`.`notes` AS `notes`,`freemeasurements_tasks`.`MeasurementId` AS `MeasurementId`,`freemeasurements_tasks`.`TaskId` AS `Taskid`,`freemeasurements_tasks`.`OrigTaskId` AS `OrigTaskId`,`freemeasurements_tasks`.`OrigTaskRev` AS `OrigTaskRev`,`freemeasurements_tasks`.`VariantId` AS `VariantId`,`freemeasurements_tasks`.`NoProductiveTaskId` AS `NoProductiveTaskId`,`freemeasurements_tasks`.`name` AS `name`,`freemeasurements_tasks`.`description` AS `description`,`freemeasurements_tasks`.`sequence` AS `sequence`,`freemeasurements_tasks`.`workstationid` AS `workstationid`,`freemeasurements_tasks`.`quantity_planned` AS `quantity_planned`,`freemeasurements_tasks`.`quantity_produced` AS `quantity_produced`,`freemeasurements_tasks`.`status` AS `TaskStatus`,`freemeasurements_tasks`.`task_startdatereal` AS `task_startdatereal`,`freemeasurements_tasks`.`task_enddatereal` AS `task_enddatereal`,`freemeasurements_tasks`.`realleadtime_hours` AS `realleadtime_hours`,`freemeasurements_tasks`.`realworkingtime_hours` AS `realworkingtime_hours`,`freemeasurements`.`creationdate` AS `FreeMeasurement_CreationDate`,`freemeasurements`.`createdby` AS `FreeMeasurement_CreatedBy`,`freemeasurements`.`plannedstartdate` AS `plannedstartdate`,`freemeasurements`.`plannedenddate` AS `plannedenddate`,`freemeasurements`.`name` AS `MeasurementName`,`freemeasurements`.`description` AS `MeasurementDescription`,`freemeasurements`.`status` AS `FreeMeasurementsStatus`,`freemeasurements`.`serialnumber` AS `serialnumber`,`postazioni`.`name` AS `WorkstationName`,`reparti`.`idreparto` AS `DepartmentId`,`reparti`.`nome` AS `DepartmentName`,`processo`.`Name` AS `ProcessName`,`processo`.`Description` AS `ProcessDescription`,`varianti`.`nomeVariante` AS `VariantName` from ((((((`freemeasurements` join `freemeasurements_tasks` on((`freemeasurements_tasks`.`MeasurementId` = `freemeasurements`.`id`))) join `freemeasurements_tasks_events` on(((`freemeasurements_tasks`.`MeasurementId` = `freemeasurements_tasks_events`.`freemeasurementid`) and (`freemeasurements_tasks`.`TaskId` = `freemeasurements_tasks_events`.`taskid`)))) left join `postazioni` on((`freemeasurements_tasks`.`workstationid` = `postazioni`.`idpostazioni`))) left join `reparti` on((`freemeasurements`.`departmentid` = `reparti`.`idreparto`))) join `processo` on(((`freemeasurements`.`processid` = `processo`.`ProcessID`) and (`freemeasurements`.`processrev` = `processo`.`revisione`)))) join `varianti` on((`freemeasurements`.`variantid` = `varianti`.`idvariante`))) order by `freemeasurements_tasks_events`.`id` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `freemeasurements_operators_tasks_running`
--

/*!50001 DROP VIEW IF EXISTS `freemeasurements_operators_tasks_running`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `freemeasurements_operators_tasks_running` AS select distinct `freemeasurements_tasks_events2`.`user` AS `user`,`runningtasks2`.`TaskName` AS `TaskName`,`runningtasks2`.`MeasurementName` AS `MeasurementName` from ((((((select max(`runningtasks`.`id`) AS `runningtasksid`,`runningtasks`.`name` AS `TaskName`,`runningtasks`.`MeasurementName` AS `MeasurementName` from (select `freemeasurements_tasks_events`.`id` AS `id`,`freemeasurements_tasks_events`.`eventtype` AS `eventtype`,`freemeasurements_tasks`.`MeasurementId` AS `measurementid`,`freemeasurements_tasks`.`TaskId` AS `taskid`,`freemeasurements_tasks`.`name` AS `name`,`freemeasurements_tasks_events`.`eventdate` AS `eventdate`,`freemeasurements`.`name` AS `MeasurementName`,`freemeasurements_tasks_events`.`user` AS `user` from ((`freemeasurements_tasks` join `freemeasurements_tasks_events` on(((`freemeasurements_tasks`.`MeasurementId` = `freemeasurements_tasks_events`.`freemeasurementid`) and (`freemeasurements_tasks`.`TaskId` = `freemeasurements_tasks_events`.`taskid`)))) join `freemeasurements` on((`freemeasurements`.`id` = `freemeasurements_tasks`.`MeasurementId`))) where (1 = 1) order by `freemeasurements_tasks_events`.`eventdate` desc) `runningtasks` group by `runningtasks`.`taskid`,`runningtasks`.`measurementid`,`runningtasks`.`user`) `runningtasks2` join `freemeasurements_tasks_events` `freemeasurements_tasks_events2` on((`freemeasurements_tasks_events2`.`id` = `runningtasks2`.`runningtasksid`))) join `freemeasurements_tasks` on(((`freemeasurements_tasks`.`MeasurementId` = `freemeasurements_tasks_events2`.`freemeasurementid`) and (`freemeasurements_tasks`.`TaskId` = `freemeasurements_tasks_events2`.`taskid`)))) join `freemeasurements` on((`freemeasurements`.`id` = `freemeasurements_tasks`.`MeasurementId`))) join `measurementunits` on((`measurementunits`.`id` = `freemeasurements`.`measurementUnit`))) left join `postazioni` on((`postazioni`.`idpostazioni` = `freemeasurements_tasks`.`workstationid`))) where (`freemeasurements_tasks_events2`.`eventtype` = 'I') */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `freemeasurements_tasks_created_by_operators`
--

/*!50001 DROP VIEW IF EXISTS `freemeasurements_tasks_created_by_operators`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `freemeasurements_tasks_created_by_operators` AS select `freemeasurements`.`id` AS `id`,`freemeasurements`.`creationdate` AS `creationdate`,`freemeasurements`.`createdby` AS `createdby`,`freemeasurements`.`plannedstartdate` AS `plannedstartdate`,`freemeasurements`.`plannedenddate` AS `plannedenddate`,`freemeasurements`.`departmentid` AS `departmentid`,`freemeasurements`.`name` AS `MeasurementName`,`freemeasurements`.`description` AS `MeasurementDescription`,`freemeasurements`.`processid` AS `ProcessId`,`freemeasurements`.`processrev` AS `processrev`,`freemeasurements`.`variantid` AS `variantid`,`freemeasurements`.`status` AS `status`,`freemeasurements`.`serialnumber` AS `serialnumber`,`freemeasurements`.`quantity` AS `quantity`,`freemeasurements`.`measurementUnit` AS `measurementUnit`,`measurementunits`.`type` AS `type`,`freemeasurements_tasks`.`TaskId` AS `taskid`,`freemeasurements_tasks`.`OrigTaskId` AS `origtaskid`,`freemeasurements_tasks`.`OrigTaskRev` AS `origtaskrev`,`freemeasurements_tasks`.`NoProductiveTaskId` AS `noproductivetaskid`,`freemeasurements_tasks`.`name` AS `TaskName`,`freemeasurements_tasks`.`description` AS `TaskDescription`,`freemeasurements_tasks`.`sequence` AS `sequence`,`freemeasurements_tasks`.`workstationid` AS `workstationid`,`postazioni`.`name` AS `name`,`freemeasurements_tasks`.`quantity_planned` AS `quantity_planned`,`freemeasurements_tasks`.`status` AS `TaskStatus`,`processo`.`Name` AS `ProcessName`,`varianti`.`nomeVariante` AS `ProductName`,`freemeasurements_tasks`.`task_startdatereal` AS `task_startdatereal`,`freemeasurements_tasks`.`task_enddatereal` AS `task_enddatereal` from ((((((`freemeasurements` join `freemeasurements_tasks` on((`freemeasurements`.`id` = `freemeasurements_tasks`.`MeasurementId`))) left join `postazioni` on((`postazioni`.`idpostazioni` = `freemeasurements_tasks`.`workstationid`))) join `measurementunits` on((`measurementunits`.`id` = `freemeasurements`.`measurementUnit`))) join `variantiprocessi` `product` on(((`freemeasurements`.`processid` = `product`.`processo`) and (`freemeasurements`.`processrev` = `product`.`revProc`) and (`freemeasurements`.`variantid` = `product`.`variante`)))) join `processo` on(((`processo`.`ProcessID` = `product`.`processo`) and (`processo`.`revisione` = `product`.`revProc`)))) join `varianti` on((`varianti`.`idvariante` = `product`.`variante`))) where ((1 = 1) and isnull(`freemeasurements_tasks`.`NoProductiveTaskId`) and isnull(`freemeasurements_tasks`.`OrigTaskId`)) order by `freemeasurements`.`id`,`freemeasurements_tasks`.`sequence` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `freemeasurements_timespans_full_view`
--

/*!50001 DROP VIEW IF EXISTS `freemeasurements_timespans_full_view`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `freemeasurements_timespans_full_view` AS select `freemeasurements_tasks`.`MeasurementId` AS `MeasurementId`,`freemeasurements`.`name` AS `MeasurementName`,`freemeasurements_tasks`.`TaskId` AS `Taskid`,`freemeasurements_tasks`.`OrigTaskId` AS `OrigTaskId`,`freemeasurements_tasks`.`OrigTaskRev` AS `OrigTaskRev`,`freemeasurements_tasks`.`VariantId` AS `VariantId`,`freemeasurements_tasks`.`NoProductiveTaskId` AS `NoProductiveTaskId`,`freemeasurements_tasks`.`name` AS `TaskName`,`freemeasurements_tasks`.`description` AS `description`,`freemeasurements_tasks`.`sequence` AS `sequence`,`freemeasurements_tasks`.`workstationid` AS `workstationid`,`postazioni`.`name` AS `WorkstationName`,`freemeasurements_tasks`.`quantity_planned` AS `quantity_planned`,`freemeasurements_tasks`.`quantity_produced` AS `quantity_produced`,`freemeasurements_tasks`.`status` AS `status`,`freemeasurements_tasks`.`task_startdatereal` AS `task_startdatereal`,`freemeasurements_tasks`.`task_enddatereal` AS `task_enddatereal`,`freemeasurements_tasks`.`realleadtime_hours` AS `realleadtime_hours`,`freemeasurements_tasks`.`realworkingtime_hours` AS `realworkingtime_hours`,`freemeasurements_tasks_events_timespans`.`id` AS `id`,`freemeasurements_tasks_events_timespans`.`inputpoint` AS `inputpoint`,`freemeasurements_tasks_events_timespans`.`starteventid` AS `starteventid`,`freemeasurements_tasks_events_timespans`.`starteventtype` AS `starteventtype`,`freemeasurements_tasks_events_timespans`.`starteventdate` AS `starteventdate`,`freemeasurements_tasks_events_timespans`.`starteventnotes` AS `starteventnotes`,`freemeasurements_tasks_events_timespans`.`endeventid` AS `endeventid`,`freemeasurements_tasks_events_timespans`.`endeventtype` AS `endeventtype`,`freemeasurements_tasks_events_timespans`.`endeventdate` AS `endeventdate`,`freemeasurements_tasks_events_timespans`.`endeventnotes` AS `endeventnotes`,(time_to_sec(timediff(`freemeasurements_tasks_events_timespans`.`endeventdate`,`freemeasurements_tasks_events_timespans`.`starteventdate`)) / 3600) AS `Timespan_duration`,`reparti`.`idreparto` AS `DepartmentId`,`reparti`.`nome` AS `DepartmentName`,`processo`.`Name` AS `ProcessName`,`processo`.`Description` AS `ProcessDescription`,`varianti`.`nomeVariante` AS `VariantName` from ((((((`freemeasurements_tasks_events_timespans` join `freemeasurements_tasks` on(((`freemeasurements_tasks_events_timespans`.`measurementid` = `freemeasurements_tasks`.`MeasurementId`) and (`freemeasurements_tasks_events_timespans`.`taskid` = `freemeasurements_tasks`.`TaskId`)))) left join `postazioni` on((`freemeasurements_tasks`.`workstationid` = `postazioni`.`idpostazioni`))) join `freemeasurements` on((`freemeasurements_tasks`.`MeasurementId` = `freemeasurements`.`id`))) left join `reparti` on((`freemeasurements`.`departmentid` = `reparti`.`idreparto`))) join `processo` on(((`freemeasurements`.`processid` = `processo`.`ProcessID`) and (`freemeasurements`.`processrev` = `processo`.`revisione`)))) join `varianti` on((`freemeasurements`.`variantid` = `varianti`.`idvariante`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `freemeasurements_timespans_full_view_no_productive`
--

/*!50001 DROP VIEW IF EXISTS `freemeasurements_timespans_full_view_no_productive`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `freemeasurements_timespans_full_view_no_productive` AS select `freemeasurements_tasks`.`MeasurementId` AS `MeasurementId`,`freemeasurements`.`name` AS `MeasurementName`,`freemeasurements_tasks`.`TaskId` AS `Taskid`,`freemeasurements_tasks`.`OrigTaskId` AS `OrigTaskId`,`freemeasurements_tasks`.`OrigTaskRev` AS `OrigTaskRev`,`freemeasurements_tasks`.`VariantId` AS `VariantId`,`freemeasurements_tasks`.`NoProductiveTaskId` AS `NoProductiveTaskId`,`freemeasurements_tasks`.`name` AS `TaskName`,`freemeasurements_tasks`.`description` AS `description`,`freemeasurements_tasks`.`sequence` AS `sequence`,`freemeasurements_tasks`.`workstationid` AS `workstationid`,`postazioni`.`name` AS `WorkstationName`,`freemeasurements_tasks`.`quantity_planned` AS `quantity_planned`,`freemeasurements_tasks`.`quantity_produced` AS `quantity_produced`,`freemeasurements_tasks`.`status` AS `status`,`freemeasurements_tasks`.`task_startdatereal` AS `task_startdatereal`,`freemeasurements_tasks`.`task_enddatereal` AS `task_enddatereal`,`freemeasurements_tasks`.`realleadtime_hours` AS `realleadtime_hours`,`freemeasurements_tasks`.`realworkingtime_hours` AS `realworkingtime_hours`,`freemeasurements_tasks_events_timespans`.`id` AS `id`,`freemeasurements_tasks_events_timespans`.`inputpoint` AS `inputpoint`,`freemeasurements_tasks_events_timespans`.`starteventid` AS `starteventid`,`freemeasurements_tasks_events_timespans`.`starteventtype` AS `starteventtype`,`freemeasurements_tasks_events_timespans`.`starteventdate` AS `starteventdate`,`freemeasurements_tasks_events_timespans`.`starteventnotes` AS `starteventnotes`,`freemeasurements_tasks_events_timespans`.`endeventid` AS `endeventid`,`freemeasurements_tasks_events_timespans`.`endeventtype` AS `endeventtype`,`freemeasurements_tasks_events_timespans`.`endeventdate` AS `endeventdate`,`freemeasurements_tasks_events_timespans`.`endeventnotes` AS `endeventnotes`,(time_to_sec(timediff(`freemeasurements_tasks_events_timespans`.`endeventdate`,`freemeasurements_tasks_events_timespans`.`starteventdate`)) / 3600) AS `Timespan_duration`,`reparti`.`idreparto` AS `DepartmentId`,`reparti`.`nome` AS `DepartmentName`,`processo`.`Name` AS `ProcessName`,`processo`.`Description` AS `ProcessDescription`,`varianti`.`nomeVariante` AS `VariantName` from ((((((`freemeasurements_tasks_events_timespans` join `freemeasurements_tasks` on(((`freemeasurements_tasks_events_timespans`.`measurementid` = `freemeasurements_tasks`.`MeasurementId`) and (`freemeasurements_tasks_events_timespans`.`taskid` = `freemeasurements_tasks`.`TaskId`)))) left join `postazioni` on((`freemeasurements_tasks`.`workstationid` = `postazioni`.`idpostazioni`))) join `freemeasurements` on((`freemeasurements_tasks`.`MeasurementId` = `freemeasurements`.`id`))) left join `reparti` on((`freemeasurements`.`departmentid` = `reparti`.`idreparto`))) join `processo` on(((`freemeasurements`.`processid` = `processo`.`ProcessID`) and (`freemeasurements`.`processrev` = `processo`.`revisione`)))) join `varianti` on((`freemeasurements`.`variantid` = `varianti`.`idvariante`))) where (`freemeasurements_tasks`.`NoProductiveTaskId` is not null) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `freemeasurements_timespans_full_view_productive`
--

/*!50001 DROP VIEW IF EXISTS `freemeasurements_timespans_full_view_productive`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `freemeasurements_timespans_full_view_productive` AS select `freemeasurements_tasks`.`MeasurementId` AS `MeasurementId`,`freemeasurements`.`name` AS `MeasurementName`,`freemeasurements_tasks`.`TaskId` AS `Taskid`,`freemeasurements_tasks`.`OrigTaskId` AS `OrigTaskId`,`freemeasurements_tasks`.`OrigTaskRev` AS `OrigTaskRev`,`freemeasurements_tasks`.`VariantId` AS `VariantId`,`freemeasurements_tasks`.`NoProductiveTaskId` AS `NoProductiveTaskId`,`freemeasurements_tasks`.`name` AS `TaskName`,`freemeasurements_tasks`.`description` AS `description`,`freemeasurements_tasks`.`sequence` AS `sequence`,`freemeasurements_tasks`.`workstationid` AS `workstationid`,`postazioni`.`name` AS `WorkstationName`,`freemeasurements_tasks`.`quantity_planned` AS `quantity_planned`,`freemeasurements_tasks`.`quantity_produced` AS `quantity_produced`,`freemeasurements_tasks`.`status` AS `status`,`freemeasurements_tasks`.`task_startdatereal` AS `task_startdatereal`,`freemeasurements_tasks`.`task_enddatereal` AS `task_enddatereal`,`freemeasurements_tasks`.`realleadtime_hours` AS `realleadtime_hours`,`freemeasurements_tasks`.`realworkingtime_hours` AS `realworkingtime_hours`,`freemeasurements_tasks_events_timespans`.`id` AS `id`,`freemeasurements_tasks_events_timespans`.`inputpoint` AS `inputpoint`,`freemeasurements_tasks_events_timespans`.`starteventid` AS `starteventid`,`freemeasurements_tasks_events_timespans`.`starteventtype` AS `starteventtype`,`freemeasurements_tasks_events_timespans`.`starteventdate` AS `starteventdate`,`freemeasurements_tasks_events_timespans`.`starteventnotes` AS `starteventnotes`,`freemeasurements_tasks_events_timespans`.`endeventid` AS `endeventid`,`freemeasurements_tasks_events_timespans`.`endeventtype` AS `endeventtype`,`freemeasurements_tasks_events_timespans`.`endeventdate` AS `endeventdate`,`freemeasurements_tasks_events_timespans`.`endeventnotes` AS `endeventnotes`,(time_to_sec(timediff(`freemeasurements_tasks_events_timespans`.`endeventdate`,`freemeasurements_tasks_events_timespans`.`starteventdate`)) / 3600) AS `Timespan_duration`,`reparti`.`idreparto` AS `DepartmentId`,`reparti`.`nome` AS `DepartmentName`,`processo`.`Name` AS `ProcessName`,`processo`.`Description` AS `ProcessDescription`,`varianti`.`nomeVariante` AS `VariantName` from ((((((`freemeasurements_tasks_events_timespans` join `freemeasurements_tasks` on(((`freemeasurements_tasks_events_timespans`.`measurementid` = `freemeasurements_tasks`.`MeasurementId`) and (`freemeasurements_tasks_events_timespans`.`taskid` = `freemeasurements_tasks`.`TaskId`)))) left join `postazioni` on((`freemeasurements_tasks`.`workstationid` = `postazioni`.`idpostazioni`))) join `freemeasurements` on((`freemeasurements_tasks`.`MeasurementId` = `freemeasurements`.`id`))) left join `reparti` on((`freemeasurements`.`departmentid` = `reparti`.`idreparto`))) join `processo` on(((`freemeasurements`.`processid` = `processo`.`ProcessID`) and (`freemeasurements`.`processrev` = `processo`.`revisione`)))) join `varianti` on((`freemeasurements`.`variantid` = `varianti`.`idvariante`))) where isnull(`freemeasurements_tasks`.`NoProductiveTaskId`) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `microsteps_value`
--

/*!50001 DROP VIEW IF EXISTS `microsteps_value`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `microsteps_value` AS select `macrostep`.`ProcessID` AS `Macro_ProcessID`,`macrostep`.`revisione` AS `revisione`,`macrostep_rel`.`variante` AS `variante`,`macrostep`.`Name` AS `Macro_Name`,`macrostep`.`Description` AS `Macro_Description`,`macro_cycletimes`.`num_op` AS `num_op`,`macro_cycletimes`.`setup` AS `setup`,`macro_cycletimes`.`tempo` AS `tempo`,`macro_cycletimes`.`tunload` AS `tunload`,`microsteps`.`processid` AS `Micro_ProcessID`,`microsteps`.`name` AS `Micro_Name`,`microsteps`.`description` AS `Micro_Description` from (((`processipadrifigli` `macrostep_rel` join `processo` `macrostep` on(((`macrostep`.`ProcessID` = `macrostep_rel`.`task`) and (`macrostep`.`revisione` = `macrostep_rel`.`revTask`)))) left join (select `microstep`.`ProcessID` AS `processid`,`microstep_rel`.`padre` AS `padre`,`microstep_rel`.`revPadre` AS `revPadre`,`microstep`.`Name` AS `name`,`microstep`.`Description` AS `description` from (`processipadrifigli` `microstep_rel` join `processo` `microstep` on(((`microstep`.`ProcessID` = `microstep_rel`.`task`) and (`microstep`.`revisione` = `microstep_rel`.`revTask`))))) `microsteps` on(((`macrostep_rel`.`task` = `microsteps`.`padre`) and (`macrostep_rel`.`revTask` = `microsteps`.`revPadre`)))) left join `tempiciclo` `macro_cycletimes` on(((`macrostep_rel`.`task` = `macro_cycletimes`.`processo`) and (`macrostep_rel`.`revTask` = `macro_cycletimes`.`revisione`) and (`macrostep_rel`.`variante` = `macro_cycletimes`.`variante`)))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `model_microsteps`
--

/*!50001 DROP VIEW IF EXISTS `model_microsteps`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `model_microsteps` AS select `productline`.`Name` AS `ProductLine_Name`,`product`.`nomeVariante` AS `Product_Name`,`macrostep`.`ProcessID` AS `Macrostep_ProcessID`,`macrostep`.`revisione` AS `revisione`,`macrostep_rel`.`variante` AS `variante`,`macrostep`.`Name` AS `Macrostep_Name`,`macrostep`.`Description` AS `Macro_Description`,`macro_cycletimes`.`num_op` AS `num_op`,`macro_cycletimes`.`setup` AS `setup`,`macro_cycletimes`.`tempo` AS `tempo`,`macro_cycletimes`.`tunload` AS `tunload`,`microsteps`.`name` AS `Microstep_Name`,`microsteps`.`description` AS `Microstep_Description`,`microsteps`.`sequence` AS `Microstep_Sequence`,`microsteps`.`cycletime` AS `Microstep_CycleTime`,if((`microsteps`.`value_or_waste` = 'V'),'Value',if((`microsteps`.`value_or_waste` = 'W'),'Evident Waste',if((`microsteps`.`value_or_waste` = 'H'),'Hidden Waste',''))) AS `Microstep_ValueOrWaste` from ((((((`processipadrifigli` `macrostep_rel` join `processo` `productline` on(((`productline`.`ProcessID` = `macrostep_rel`.`padre`) and (`productline`.`revisione` = `macrostep_rel`.`revPadre`)))) join `variantiprocessi` `variantiprocessi_product` on(((`productline`.`ProcessID` = `variantiprocessi_product`.`processo`) and (`productline`.`revisione` = `variantiprocessi_product`.`revProc`) and (`macrostep_rel`.`variante` = `variantiprocessi_product`.`variante`)))) join `varianti` `product` on((`variantiprocessi_product`.`variante` = `product`.`idvariante`))) join `processo` `macrostep` on(((`macrostep`.`ProcessID` = `macrostep_rel`.`task`) and (`macrostep`.`revisione` = `macrostep_rel`.`revTask`)))) left join `tempiciclo` `macro_cycletimes` on(((`macrostep_rel`.`task` = `macro_cycletimes`.`processo`) and (`macrostep_rel`.`revTask` = `macro_cycletimes`.`revisione`) and (`macrostep_rel`.`variante` = `macro_cycletimes`.`variante`)))) left join (select `task_microsteps`.`taskid` AS `taskid`,`task_microsteps`.`taskrev` AS `taskrev`,`task_microsteps`.`variantid` AS `variantid`,`task_microsteps`.`microstep_id` AS `microstep_id`,`task_microsteps`.`microstep_rev` AS `microstep_rev`,`task_microsteps`.`sequence` AS `sequence`,`task_microsteps`.`cycletime` AS `cycletime`,`task_microsteps`.`value_or_waste` AS `value_or_waste`,`microsteps`.`id` AS `id`,`microsteps`.`review` AS `review`,`microsteps`.`name` AS `name`,`microsteps`.`description` AS `description`,`microsteps`.`creation_date` AS `creation_date` from (`task_microsteps` join `microsteps` on(((`task_microsteps`.`microstep_id` = `microsteps`.`id`) and (`task_microsteps`.`microstep_rev` = `microsteps`.`review`))))) `microsteps` on(((`microsteps`.`taskid` = `macrostep_rel`.`task`) and (`microsteps`.`taskrev` = `macrostep_rel`.`revTask`) and (`macrostep_rel`.`variante` = `microsteps`.`variantid`)))) where (1 = 1) order by `product`.`idvariante`,`macrostep_rel`.`posx`,`microsteps`.`sequence` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `model_microsteps_concat`
--

/*!50001 DROP VIEW IF EXISTS `model_microsteps_concat`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `model_microsteps_concat` AS select `productline`.`Name` AS `ProductLine_Name`,`product`.`nomeVariante` AS `Product_Name`,`macrostep`.`ProcessID` AS `Macro_ProcessID`,`macrostep`.`revisione` AS `Macro_Review`,`macrostep_rel`.`variante` AS `Macro_Product`,`macrostep`.`Name` AS `Macro_Name`,`macrostep`.`Description` AS `Macro_Description`,`macro_cycletimes`.`num_op` AS `num_op`,`macro_cycletimes`.`setup` AS `setup`,`macro_cycletimes`.`tempo` AS `tempo`,`macro_cycletimes`.`tunload` AS `tunload`,group_concat(`microsteps`.`name` separator ', ') AS `Microsteps_Name`,sum(`microsteps`.`cycletime`) AS `Microstep_CycleTime` from ((((((`processipadrifigli` `macrostep_rel` join `processo` `productline` on(((`productline`.`ProcessID` = `macrostep_rel`.`padre`) and (`productline`.`revisione` = `macrostep_rel`.`revPadre`)))) join `variantiprocessi` `variantiprocessi_product` on(((`productline`.`ProcessID` = `variantiprocessi_product`.`processo`) and (`productline`.`revisione` = `variantiprocessi_product`.`revProc`) and (`macrostep_rel`.`variante` = `variantiprocessi_product`.`variante`)))) join `varianti` `product` on((`variantiprocessi_product`.`variante` = `product`.`idvariante`))) join `processo` `macrostep` on(((`macrostep`.`ProcessID` = `macrostep_rel`.`task`) and (`macrostep`.`revisione` = `macrostep_rel`.`revTask`)))) left join `tempiciclo` `macro_cycletimes` on(((`macrostep_rel`.`task` = `macro_cycletimes`.`processo`) and (`macrostep_rel`.`revTask` = `macro_cycletimes`.`revisione`) and (`macrostep_rel`.`variante` = `macro_cycletimes`.`variante`)))) left join (select `task_microsteps`.`taskid` AS `taskid`,`task_microsteps`.`taskrev` AS `taskrev`,`task_microsteps`.`variantid` AS `variantid`,`task_microsteps`.`microstep_id` AS `microstep_id`,`task_microsteps`.`microstep_rev` AS `microstep_rev`,`task_microsteps`.`sequence` AS `sequence`,`task_microsteps`.`cycletime` AS `cycletime`,`task_microsteps`.`value_or_waste` AS `value_or_waste`,`microsteps`.`id` AS `id`,`microsteps`.`review` AS `review`,`microsteps`.`name` AS `name`,`microsteps`.`description` AS `description`,`microsteps`.`creation_date` AS `creation_date` from (`task_microsteps` join `microsteps` on(((`task_microsteps`.`microstep_id` = `microsteps`.`id`) and (`task_microsteps`.`microstep_rev` = `microsteps`.`review`))))) `microsteps` on(((`microsteps`.`taskid` = `macrostep_rel`.`task`) and (`microsteps`.`taskrev` = `macrostep_rel`.`revTask`) and (`macrostep_rel`.`variante` = `microsteps`.`variantid`)))) where (1 = 1) group by `Macro_ProcessID`,`Macro_Review`,`Macro_Product` order by `product`.`idvariante`,`macrostep_rel`.`posx`,`microsteps`.`sequence` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-10-17 12:10:55
