-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: localhost    Database: vcmain
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
-- Table structure for table `configurazione`
--

DROP TABLE IF EXISTS `configurazione`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `configurazione` (
  `WorkspaceId` int(11) NOT NULL,
  `Sezione` varchar(255) COLLATE utf32_bin NOT NULL,
  `ID` int(11) NOT NULL,
  `parametro` varchar(255) COLLATE utf32_bin NOT NULL,
  `valore` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  PRIMARY KEY (`WorkspaceId`,`Sezione`,`ID`,`parametro`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `configurazione`
--

LOCK TABLES `configurazione` WRITE;
/*!40000 ALTER TABLE `configurazione` DISABLE KEYS */;
/*!40000 ALTER TABLE `configurazione` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groupspermissions`
--

DROP TABLE IF EXISTS `groupspermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `groupspermissions` (
  `groupid` int(11) NOT NULL,
  `permissionid` int(11) NOT NULL,
  `r` bit(1) DEFAULT NULL,
  `w` bit(1) DEFAULT NULL,
  `x` bit(1) DEFAULT NULL,
  PRIMARY KEY (`groupid`,`permissionid`),
  KEY `permission_FK1_idx` (`permissionid`),
  CONSTRAINT `group_FK1` FOREIGN KEY (`groupid`) REFERENCES `groupss` (`id`),
  CONSTRAINT `permission_FK1` FOREIGN KEY (`permissionid`) REFERENCES `permissions` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groupspermissions`
--

LOCK TABLES `groupspermissions` WRITE;
/*!40000 ALTER TABLE `groupspermissions` DISABLE KEYS */;
INSERT INTO `groupspermissions` VALUES (4,105,'','',''),(5,106,'','',''),(13,4,'','',''),(13,5,'','',''),(13,6,'','',''),(13,7,'','',''),(13,8,'','',''),(13,9,'','',''),(13,10,'','',''),(13,11,'','',''),(13,12,'','',''),(13,13,'','',''),(13,14,'','',''),(13,15,'','',''),(13,16,'','',''),(13,17,'','',''),(13,18,'','',''),(13,19,'','',''),(13,20,'','',''),(13,21,'','',''),(13,22,'','',''),(13,23,'','',''),(13,24,'','',''),(13,25,'','',''),(13,26,'','',''),(13,27,'','',''),(13,28,'','',''),(13,29,'','',''),(13,30,'','',''),(13,31,'','',''),(13,32,'','',''),(13,33,'','',''),(13,34,'','',''),(13,35,'','',''),(13,36,'','',''),(13,37,'','','\0'),(13,38,'','',''),(13,39,'\0','','\0'),(13,40,'','','\0'),(13,41,'','','\0'),(13,42,'','','\0'),(13,43,'','','\0'),(13,44,'','','\0'),(13,45,'','','\0'),(13,46,'','','\0'),(13,47,'','',''),(13,48,'','\0','\0'),(13,49,'','','\0'),(13,50,'','\0','\0'),(13,51,'','\0','\0'),(13,52,'','\0','\0'),(13,53,'','',''),(13,54,'','\0','\0'),(13,55,'\0','\0',''),(13,56,'\0','','\0'),(13,57,'','',''),(13,58,'','',''),(13,59,'','',''),(13,60,'\0','','\0'),(13,61,'\0','','\0'),(13,62,'','',''),(13,63,'','',''),(13,64,'\0','','\0'),(13,65,'','',''),(13,66,'','',''),(13,67,'','',''),(13,68,'','',''),(13,69,'','',''),(13,70,'','',''),(13,71,'','',''),(13,72,'','',''),(13,73,'','',''),(13,74,'','',''),(13,76,'','',''),(13,84,'','',''),(13,85,'','',''),(13,87,'','','\0'),(13,88,'','',''),(13,89,'','',''),(13,90,'','',''),(13,93,'','',''),(13,94,'','',''),(13,95,'','',''),(13,96,'','',''),(13,97,'','',''),(13,98,'','',''),(13,99,'','\0','\0'),(13,100,'','',''),(13,101,'','',''),(13,102,'','',''),(13,103,'','',''),(13,104,'','',''),(13,107,'','',NULL),(13,108,'','',''),(13,109,'','',''),(14,43,'','','\0'),(14,102,'','',''),(14,103,'','\0','\0');
/*!40000 ALTER TABLE `groupspermissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groupss`
--

DROP TABLE IF EXISTS `groupss`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `groupss` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) COLLATE utf32_bin NOT NULL,
  `description` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groupss`
--

LOCK TABLES `groupss` WRITE;
/*!40000 ALTER TABLE `groupss` DISABLE KEYS */;
INSERT INTO `groupss` VALUES (0,'Admin','Gruppo amministrativo'),(1,'Plant management','Gestione stabilimento produttivo'),(2,'Commerciale','Gestione ordini da cliente'),(3,'Ufficio Tecnico','Ufficio tecnico'),(4,'Pianificazione produzione','Pianificazione produzione'),(5,'Operatori','Gruppo degli operatori'),(6,'Quality manager','Responsabile qualita'),(7,'Operatore qualita','Adetto alla qualita'),(8,'CustomerUser','Group of customers users'),(13,'WorkspaceAdmin','Account administrator'),(14,'Basic account','Basic Account');
/*!40000 ALTER TABLE `groupss` ENABLE KEYS */;
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
  KEY `menualbero_FK2_idx` (`idFiglio`),
  CONSTRAINT `menualbero_FK1` FOREIGN KEY (`idPadre`) REFERENCES `menuvoci` (`id`),
  CONSTRAINT `menualbero_FK2` FOREIGN KEY (`idFiglio`) REFERENCES `menuvoci` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menualbero`
--

LOCK TABLES `menualbero` WRITE;
/*!40000 ALTER TABLE `menualbero` DISABLE KEYS */;
INSERT INTO `menualbero` VALUES (2,41,0),(2,42,6),(2,48,7),(2,49,8),(2,50,9),(2,69,5),(5,63,0),(5,64,1),(8,43,0),(8,44,1),(8,45,3),(9,24,0),(9,32,2),(9,33,1),(9,39,1),(10,26,0),(10,28,0),(34,35,0),(34,36,0);
/*!40000 ALTER TABLE `menualbero` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menugroups`
--

DROP TABLE IF EXISTS `menugroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `menugroups` (
  `groupid` int(11) NOT NULL,
  `menuitemid` int(11) NOT NULL,
  `sequence` int(11) NOT NULL,
  PRIMARY KEY (`groupid`,`menuitemid`),
  KEY `menugroups_FK7_idx` (`menuitemid`),
  CONSTRAINT `menugroups_FK1` FOREIGN KEY (`groupid`) REFERENCES `groupss` (`id`),
  CONSTRAINT `menugroups_FK7` FOREIGN KEY (`menuitemid`) REFERENCES `menuvoci` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menugroups`
--

LOCK TABLES `menugroups` WRITE;
/*!40000 ALTER TABLE `menugroups` DISABLE KEYS */;
INSERT INTO `menugroups` VALUES (13,2,1),(13,5,5),(13,8,5),(13,9,5),(13,10,5),(13,18,0),(13,34,5),(13,46,5),(13,47,5),(13,51,5);
/*!40000 ALTER TABLE `menugroups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menuvoci`
--

DROP TABLE IF EXISTS `menuvoci`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `menuvoci` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf32_bin NOT NULL,
  `description` text COLLATE utf32_bin,
  `url` varchar(255) COLLATE utf32_bin NOT NULL,
  `creationdate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menuvoci`
--

LOCK TABLES `menuvoci` WRITE;
/*!40000 ALTER TABLE `menuvoci` DISABLE KEYS */;
INSERT INTO `menuvoci` VALUES (1,'Login','Login','~/Login/login.aspx','2021-03-20 21:53:59'),(2,'Admin','Admin','~/admin/admin.aspx','2021-03-23 22:32:49'),(5,'Product Manager','Process Manager','~/Processi/MacroProcessi.aspx','2021-03-23 22:32:49'),(8,'Clienti','Customers menu','~/Commesse/commesse.aspx','2021-03-23 22:32:49'),(9,'Production plan','Production plan','~/Produzione/produzione.aspx','2021-03-23 22:32:49'),(10,'WorkPlace','WorkPlace','~/Operatori/checkInPostazione.aspx','2021-03-23 22:32:49'),(18,'Login','Login','~/Login/login.aspx','2021-03-23 22:32:49'),(24,'Nuove commesse','Nuove commesse','~/Produzione/commesseDaProdurre.aspx','2021-03-23 22:32:49'),(26,'Barcode Gemba','Interfaccia barcode per operatori via javascript','~/Operatori/AzioniBarcodeJS.aspx','2021-03-23 22:32:49'),(28,'Web Gemba','Interfaccia di input operatori completamente via web','~/Workplace/WebGemba/Index','2021-03-23 22:32:49'),(32,'Piano Produzione','Piano produttivo completo','~/Production/ProductionSchedule/Index','2021-03-23 22:32:49'),(33,'Storico produzione','Storico produzione','~/Analysis/ProductionHistory/Index','2021-03-23 22:32:49'),(34,'Andon','Andon','~/Produzione/AndonListReparti.aspx','2021-03-23 22:32:49'),(35,'Andon Reparto','Andon Reparto','~/Produzione/AndonListReparti.aspx','2021-03-23 22:32:49'),(36,'Andon Generale','Andon Generale','~/Produzione/avanzamentoProduzione.aspx','2021-03-23 22:32:49'),(39,'Carico di lavoro','Gestione e simulazione carico di lavoro per reparto','~/Analysis/ProductionWorkload/Index','2021-03-23 22:32:49'),(41,'Configurazione principale','Menu di configurazione del sistema generale','~/Admin/kisAdmin.aspx','2021-03-23 22:32:49'),(42,'Gestione Andon Completo','Gestione Andon Completo','~/Andon/configAndonCompleto.aspx','2021-03-23 22:32:49'),(43,'Anagrafica clienti','Anagrafica clienti','~/Customers/Customer/List','2021-03-23 22:32:49'),(44,'Ordini','Ordini','~/Commesse/commesse.aspx','2021-03-23 22:32:49'),(45,'Wizard nuovo ordine','Wizard prodotto su commessa	','~/Commesse/wzAddCommessa.aspx','2021-03-23 22:32:49'),(46,'Analisi dati','Analisi dati','~/Analysis/Analysis.aspx','2021-03-23 22:32:49'),(47,'myArea','Gestione account personale','~/Personal/my.aspx','2021-03-23 22:32:49'),(48,'Configurazione Reports','Configurazione reports','~/Admin/manageReports.aspx','2021-03-23 22:32:49'),(49,'Configurazione Reparti','Gestione reparti','~/Reparti/listReparti.aspx','2021-03-23 22:32:49'),(50,'Configurazione Postazioni di Lavoro','Configurazione Postazioni di Lavoro','~/Postazioni/managePostazioniLavoro.aspx','2021-03-23 22:32:49'),(51,'Qualità','Gestione qualità','~/Quality/Home/Index','2021-03-23 22:32:49'),(52,'Unita di misura','Gestione unita di misura','~/Config/MeasurementUnits/Index','2021-03-23 22:32:49'),(62,'Task Non Produttivi','Task non produttivi','~/Config/NoProductiveTasks/Index','2021-03-23 22:32:49'),(63,'Process Manager','Process Manager','~/Processi/MacroProcessi.aspx','2021-03-23 22:32:49'),(64,'Work Instructions','Work Instructions','~/WorkInstructions/WorkInstructions/Index','2021-03-23 22:32:49'),(65,'Free Measurements','Free Measurements','~/FreeTimeMeasurement/FreeMeasurement/ChooseDepartment','2021-03-23 22:32:49'),(66,'Free Measurements','Free Measurements','~/FreeTimeMeasurement/FreeMeasurement/Index','2021-03-23 22:32:49'),(67,'FreeMeasurements','FreeMeasurements','~/FreeTimeMeasurement/FreeMeasurement/Index','2021-03-23 22:32:49'),(68,'Task non produttivi','Task non produttivi','~/Config/NoProductiveTasks/Index','2021-03-23 22:32:49'),(69,'Input Points','Input Points','~/Production/InputPoints/Index','2021-08-29 17:12:00');
/*!40000 ALTER TABLE `menuvoci` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `old_groupusers`
--

DROP TABLE IF EXISTS `old_groupusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `old_groupusers` (
  `groupid` int(11) NOT NULL,
  `useraccountid` int(11) NOT NULL,
  `workspaceid` int(11) NOT NULL,
  PRIMARY KEY (`groupid`,`useraccountid`,`workspaceid`),
  KEY `groupusers_FK2_idx` (`useraccountid`),
  KEY `groupusers_FK3_idx` (`workspaceid`),
  CONSTRAINT `groupusers_FK1` FOREIGN KEY (`groupid`) REFERENCES `groupss` (`id`),
  CONSTRAINT `groupusers_FK2` FOREIGN KEY (`useraccountid`) REFERENCES `useraccounts` (`id`),
  CONSTRAINT `groupusers_FK3` FOREIGN KEY (`workspaceid`) REFERENCES `workspaces` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `old_groupusers`
--

LOCK TABLES `old_groupusers` WRITE;
/*!40000 ALTER TABLE `old_groupusers` DISABLE KEYS */;
/*!40000 ALTER TABLE `old_groupusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permissions`
--

DROP TABLE IF EXISTS `permissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `permissions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) COLLATE utf32_bin NOT NULL,
  `description` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=110 DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permissions`
--

LOCK TABLES `permissions` WRITE;
/*!40000 ALTER TABLE `permissions` DISABLE KEYS */;
INSERT INTO `permissions` VALUES (4,'Postazione check-in','Permesso di eseguire il check-in su una postazione di lavoro'),(5,'Task Produzione','Visione, gestione, esecuzione task di produzione'),(6,'Utenti','Gestione utenti'),(7,'Gruppi','Gestione gruppi'),(8,'Gruppi Permessi','Associazione dei permessi ai gruppi'),(9,'Menu Voce','Gestione delle voci di menu'),(10,'Gruppi Menu','Gestisce l&#39;associazione delle voci di menu ai gruppi'),(11,'Commesse','Gestione commesse'),(12,'Articoli','Gestione articoli in produzione'),(13,'Postazione','Gestione postazioni'),(14,'Calendario Postazione','Gestisce il calendario di postazione'),(15,'Postazioni Operatori','Gestisce gli operatori in postazione'),(16,'Processo','Gestione processi'),(17,'Reparto ProcessoVariante','Gestione Reparti - ProcessoVariante'),(18,'Processo TempiCiclo','Gestione tempi ciclo nei processi'),(19,'Processo Variante','Gestione dei Processo Variante'),(20,'Reparto','Gestione reparti'),(21,'Warning','GEstione degli warning'),(22,'Turno','Gestione turni di lavoro'),(23,'Reparto Operatori','Gestione operatori di reparto.'),(24,'Reparto Straordinari','Gestione straordinari di reparto.'),(25,'Reparto Festivita','Gestione festivit&#224; di reparto.'),(26,'Permessi','Gestione permessi'),(27,'Task Postazione','Associazione dei tasks alle postazioni di lavoro'),(28,'Reparto WorkLoad','Gestione carico di lavoro di un reparto'),(29,'Utenti E-mail','Gestione indirizzi e-mail degli utenti'),(30,'Utenti PhoneNumbers','Gestione dei numeri telefonici degli utenti'),(31,'Reparto EventoRitardo','Gestione ritardi per reparto'),(32,'Reparto EventoWarning','Gestione eventi warning all&#39;interno dei reparti'),(33,'Commessa EventoRitardo','Gestione della configurazione dei ritardi per la commessa'),(34,'Commessa EventoWarning','Gestione degli eventi warning di commessa'),(35,'Articolo EventoRitardo','Gestione configurazione eventi di ritardo per articolo'),(36,'Articolo EventoWarning','Gestione configurazione eventi segnalazione di warning per articolo'),(37,'Reparto ModoCalcoloTC','Impostare il modo di calcolo del tempo ciclo'),(38,'Articolo Depianifica','Rimuovere un prodotto dal piano produzione'),(39,'Configurazione Logo','Configurazione Logo'),(40,'Reparto Andon VisualizzazioneNomiUtente','Configurazione visualizzazione nomi su andon'),(41,'AndonCompleto VisualizzazioneNomiUtente','Configurazione visualizzazione nomi utente su andon'),(42,'Reparto AvvioTasksOperatori','Gestione del numero massimo di tasks avviabili dagli operatori'),(43,'Anagrafica Clienti','Anagrafica Clienti'),(44,'Andon Reparto VisualizzazioneGiorni','Andon Reparto VisualizzazioneGiorni'),(45,'Andon Reparto','Visualizzazione andon reparto'),(46,'Anagrafica Clienti Contatti','Anagrafica Clienti Contatti'),(47,'Analisi Commessa Costo','Analisi Commessa Costo'),(48,'Analisi Operatori Tempi','Analisi Operatori Tempi'),(49,'Analisi','Interfaccia analisi dati'),(50,'Analisi Articolo Costo','Analisi Articolo Costo'),(51,'Analisi TipoProdotto','Accesso all&#39;interfaccia di analisi per tipo di prodotto'),(52,'Analisi Tasks','Accesso all&#39;interfaccia di analisi dei tasks'),(53,'Wizard TipoPERT','Permesso di variare la tipologia di visualizzazione della creazione processi, da PERT a Tabella e viceversa'),(54,'Analisi Clienti','Permesso per visualizzare interfaccia analisi tempi di lavoro clienti'),(55,'Report Stato Ordini Clienti','Report Stato Ordini Clienti'),(56,'Configurazione Report Stato Ordini Clienti','Configurazione Report Stato Ordini Clienti'),(57,'Turno PostazioneRisorse','Gestione della capacit&#224; produttiva per turno'),(58,'AndonCompleto CampiDaVisualizzare','Gestione campi visualizzati su Andon'),(59,'AndonReparto CampiDaVisualizzare','Gestione campi da visualizzare su Andon Reparto'),(60,'Configurazione TimeZone','Permette di configurare il fuso orario'),(61,'Reparto Timezone','Permette di configurare il fuso orario per singolo reparto'),(62,'Articolo Riesuma','Articolo Riesuma'),(63,'TaskProduzione Riesuma','TaskProduzione Riesuma'),(64,'Reparto ConfigurazioneKanban','Abilitazione KanbanBox by Sintesia'),(65,'NonCompliance Types','Categorie di non conformita'),(66,'NonCompliance Causes','Cause di non conformita'),(67,'NonCompliances','Non conformita'),(68,'NonCompliancesAnalysis Product','NonCompliancesAnalysis Product'),(69,'NonCompliancesAnalysis Num','NonCompliancesAnalysis Num'),(70,'NonCompliancesAnalysis Cost','NonCompliancesAnalysis Cost'),(71,'ImprovementActions','ImprovementActions'),(72,'Billing','Billing service'),(73,'Product ParameterCategories','Product ParameterCategories'),(74,'Product Parameters','Product Parameters'),(75,'Customer Order','Permission given to the customer to make an online order'),(76,'Task Parameter','Set task parameter\'s value'),(84,'AndonCompleto ScrollType','AndonCompleto ScrollType'),(85,'Working Hours Manual Registration','Working Hours Manual Registration'),(87,'Config MeasurementUnits','Measurement Units management authorization'),(88,'WorkInstructions Manage','WorkInstructions Manage'),(89,'Task WorkInstructions','Task WorkInstructions'),(90,'AndonReparto Config','AndonReparto Config'),(93,'Task DefaultOperators','Task DefaultOperators'),(94,'Tasks ManageOperators','Tasks ManageOperators'),(95,'User Disable','Disables / enables a user'),(96,'DisabledUsers','DisabledUsers'),(97,'Tasks ManuallyReschedule','Tasks ManuallyReschedule'),(98,'Reparto AllowTaskOperatorsComments','Reparto AllowTaskOperatorsComments'),(99,'Process Mining','Process Mining'),(100,'Config SalesOrdersImport3rdPartySystem','Config SalesOrdersImport3rdPartySystem'),(101,'Workspaces Invite','Workspaces Invite'),(102,'User Workspaces','User Workspaces'),(103,'Workspace Details','Workspace Details'),(104,'Workspaces UserAccountsGroups','Workspaces UserAccountsGroups'),(105,'FreeMeasurement Manage','FreeMeasurement Manage'),(106,'FreeMeasurement ExecuteTasks','FreeMeasurement ExecuteTasks'),(107,'InputPoints List','InputPoints List'),(108,'InputPoint Details','InoutPoint Details'),(109,'InputPoint check-in','InputPoint check-in');
/*!40000 ALTER TABLE `permissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `useraccounts`
--

DROP TABLE IF EXISTS `useraccounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `useraccounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  `email` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  `firstname` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  `lastname` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  `nickname` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  `picture_url` varchar(1000) COLLATE utf32_bin DEFAULT NULL,
  `locale` varchar(45) COLLATE utf32_bin DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `iss` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  `nonce` text COLLATE utf32_bin,
  `id_token` text COLLATE utf32_bin,
  `access_token` text COLLATE utf32_bin,
  `refresh_token` text COLLATE utf32_bin,
  `created_at` datetime DEFAULT NULL,
  `lastlogin` datetime DEFAULT CURRENT_TIMESTAMP,
  `globaladmin` bit(1) DEFAULT b'0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useraccounts`
--

LOCK TABLES `useraccounts` WRITE;
/*!40000 ALTER TABLE `useraccounts` DISABLE KEYS */;
INSERT INTO `useraccounts` VALUES (7,'google-oauth2|106708164772575417205','mgrisoster@gmail.com','Matteo Griso','Griso','mgrisoster','https://lh3.googleusercontent.com/a-/AOh14Ggk3Wqb5yypa5-IHJ9LBmT4Iwy9qg7xf_ZoW3uceg=s96-c','en','2020-12-16 21:37:14','https://virtualchief.eu.auth0.com/','637437622197841336.MTM0YjZkZGEtZmI1YS00ZDQ1LTg5ODUtNTUzZWQyMGY5NjFjODc3MTY2NDctZWJiZC00ZDQ1LWE3ODUtN2JhODk3YjNhNmY1','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJnaXZlbl9uYW1lIjoiTWF0dGVvIiwiZmFtaWx5X25hbWUiOiJHcmlzbyIsIm5pY2tuYW1lIjoibWdyaXNvc3RlciIsIm5hbWUiOiJNYXR0ZW8gR3Jpc28iLCJwaWN0dXJlIjoiaHR0cHM6Ly9saDMuZ29vZ2xldXNlcmNvbnRlbnQuY29tL2EtL0FPaDE0R2drM1dxYjV5eXBhNS1JSEo5TEJtVDRJd3k5cWc3eGZfWm9XM3VjZWc9czk2LWMiLCJsb2NhbGUiOiJlbiIsInVwZGF0ZWRfYXQiOiIyMDIwLTEyLTE3VDAwOjM3OjE0LjA2MFoiLCJlbWFpbCI6Im1ncmlzb3N0ZXJAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImlzcyI6Imh0dHBzOi8vdmlydHVhbGNoaWVmLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJnb29nbGUtb2F1dGgyfDEwNjcwODE2NDc3MjU3NTQxNzIwNSIsImF1ZCI6IjFoQmdoWDNZcTB4S004NkVya0c0bjZYdXJ2UjVTRzUyIiwiaWF0IjoxNjA4MTY1NDM0LCJleHAiOjE2MDgyMDE0MzQsImF0X2hhc2giOiJNbFdNLXVyOGIxRVBtMWw0ajJ3TFRBIiwiY19oYXNoIjoiUGl5SUJ3NFJZbVBhcFpIWTUwcUI1USIsIm5vbmNlIjoiNjM3NDM3NjIyMTk3ODQxMzM2Lk1UTTBZalprWkdFdFptSTFZUzAwWkRRMUxUZzVPRFV0TlRVelpXUXlNR1k1TmpGak9EYzNNVFkyTkRjdFpXSmlaQzAwWkRRMUxXRTNPRFV0TjJKaE9EazNZak5oTm1ZMSJ9.lCDX6KcFNqSe9gNBl6mmtWLBuE50lGzBB-HZHW2bhkA6wuYS1YG4leJRLVtoY362jnDG5OikuSRf5qfPSnvy5o4tuJX9m2xPP5JRSW3ITvscOsERQHDC2V4CcHCxRhScjlefVFnHwK4QYt4ZTckkJia-dHnjmg-0VV6Lz8OzMSN7v3S3QjsRikW6LJJ68SXvRBXDUJ8QEvL_Ti8b2WlTcsqH1RY-MBgiaOIcdGCEBHLqG6AAkcj3LhMQ3__RmdTTt3Ylzx9LWshfgjP6ZMyCVftzX3HsZEyCxx_Kh3BkTru7NM83tKv6Bo0XOjPL8rlciOtMOiRPUL1IEDMX6Xd0Aw','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJpc3MiOiJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vIiwic3ViIjoiZ29vZ2xlLW9hdXRoMnwxMDY3MDgxNjQ3NzI1NzU0MTcyMDUiLCJhdWQiOlsiaHR0cHM6Ly92aXJ0dWFsY2hpZWYuZXUuYXV0aDAuY29tL2FwaS92Mi8iLCJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vdXNlcmluZm8iXSwiaWF0IjoxNjA4MTY1NDM0LCJleHAiOjE2MDgxNzI2MzQsImF6cCI6IjFoQmdoWDNZcTB4S004NkVya0c0bjZYdXJ2UjVTRzUyIiwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCJ9.D5PVni71hX-qsTKrs7-y5GaTSCTPwZ5FZUh4DuY_DE5XbmzfznJr-vPIu-B-HCPDTWdz30h94o7v25PL2A5_9OwUMPQNUeGB1goetqEFPTo2029mvKS3K4B1qRXHt71MlPFHcNyBcbmm5R31PEvDscPv0UNT1RKo0oC4WgqkBB3Gl32o2oCjEna3us449bB17t7goONTgDFcX9ES1M4XZTG9bN_akodhroagnXKLgar-s7hj2wwCnKHLDNE2r9eLeNXrLvwuFhW0uXraBQQVIzvpsQNHHyaWhoLmd0NVolhqCNZaRUbE09D0pJJC0pPZhkSuOmJ5UYKoovYJESEOlA',NULL,'2020-12-17 00:37:15','2021-09-05 14:55:35','\0'),(9,'auth0|5fdaa8ecc7f13600790086b1','m.griso@hotmail.it','m.griso@hotmail.it',NULL,'m.griso','https://s.gravatar.com/avatar/00265c673015126cb517747241e3f4ba?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fm.png',NULL,'2020-12-16 21:40:13','https://virtualchief.eu.auth0.com/','637437623985996578.MDJmZmQ5OTktOWIzNy00NWU2LWFjMGMtZTQwNDQ4OTI4NGNkMTYyNTAxNDYtN2Y4MC00NWI5LWE5YWMtNTc1MTZiOTRiYzlk','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJuaWNrbmFtZSI6Im0uZ3Jpc28iLCJuYW1lIjoibS5ncmlzb0Bob3RtYWlsLml0IiwicGljdHVyZSI6Imh0dHBzOi8vcy5ncmF2YXRhci5jb20vYXZhdGFyLzAwMjY1YzY3MzAxNTEyNmNiNTE3NzQ3MjQxZTNmNGJhP3M9NDgwJnI9cGcmZD1odHRwcyUzQSUyRiUyRmNkbi5hdXRoMC5jb20lMkZhdmF0YXJzJTJGbS5wbmciLCJ1cGRhdGVkX2F0IjoiMjAyMC0xMi0xN1QwMDo0MDoxMy41NTlaIiwiZW1haWwiOiJtLmdyaXNvQGhvdG1haWwuaXQiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImlzcyI6Imh0dHBzOi8vdmlydHVhbGNoaWVmLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJhdXRoMHw1ZmRhYThlY2M3ZjEzNjAwNzkwMDg2YjEiLCJhdWQiOiIxaEJnaFgzWXEweEtNODZFcmtHNG42WHVydlI1U0c1MiIsImlhdCI6MTYwODE2NTYyMSwiZXhwIjoxNjA4MjAxNjIxLCJhdF9oYXNoIjoiSlFZbzk5NE9hdUJBb3NJNDgxblcxQSIsImNfaGFzaCI6ImdQZ3NxRFdQTXpMRFlLczFwT0hNZXciLCJub25jZSI6IjYzNzQzNzYyMzk4NTk5NjU3OC5NREptWm1RNU9Ua3RPV0l6TnkwME5XVTJMV0ZqTUdNdFpUUXdORFE0T1RJNE5HTmtNVFl5TlRBeE5EWXROMlk0TUMwME5XSTVMV0U1WVdNdE5UYzFNVFppT1RSaVl6bGsifQ.m4R3Uc9H2b1sq7-odxWxfZ7buES4s4tNkwN_Q_bjx-_w6N0wT_nNNe_FZIiZJxQIXf_Py0_yra8kqdgDciIhvKGr-geyi98CEslsLRlJha5prt0-7u7kMqX-wTNJYnwQ2njo2DY3FdSkaqO6JNQ2FDYjzmrS_mvkL9IH4mNEVjPFJUDUY9kLbH_WLnUjhncX9I6OIuzHw7_pHlLLW-6CXlAuf2GMUrHIa3giQ2M7MQVCvPUltEKVybc7n0PJfE7AyeH4qlrRjY7HCbPUF1-ykmhB4OUx3QcaVnsv5gfqQUd6ahHOROriqIy7kz2z_8zFntz7nA17pbIpdw_sEq8Lfw','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJpc3MiOiJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vIiwic3ViIjoiYXV0aDB8NWZkYWE4ZWNjN2YxMzYwMDc5MDA4NmIxIiwiYXVkIjpbImh0dHBzOi8vdmlydHVhbGNoaWVmLmV1LmF1dGgwLmNvbS9hcGkvdjIvIiwiaHR0cHM6Ly92aXJ0dWFsY2hpZWYuZXUuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTYwODE2NTYyMSwiZXhwIjoxNjA4MTcyODIxLCJhenAiOiIxaEJnaFgzWXEweEtNODZFcmtHNG42WHVydlI1U0c1MiIsInNjb3BlIjoib3BlbmlkIHByb2ZpbGUgZW1haWwifQ.NnXjwqp2rvs9o2XMQiaA_reIK2bP0Q9SPdcUe-vVLu8zoUH2hdUE8uA1j0OhneM4iZOYIfDhGBfXWkp8bNcyoO2hyzSY4EUNT2erMV4j2tuhBWXZVb0F1InaG4GZtFLtjFOaEyXfO2Xzj7SE0SaZzCWSwh_LCXpkKnA9Dl5OtFBdeGjhZwP2cKwD5Gpd2cDK6UV2RXe3gc06dCBoe0D3tAv98EUwkgaaLY50TOk0RcKqo48dIsHY8poUT_zpskcEI6licHK6XIN2T13VHtqBlKkA2M4rCEZJJyeAKJ28U-ssAZqoExeyxFewfYzIOBTJZtjFLe2Gjoh0a4o2jag93A',NULL,'2020-12-17 00:40:22','2021-03-20 22:06:32','\0'),(13,'google-oauth2|110995565931815493426','mgriso@kaizenkey.com',NULL,'Griso','mgriso','https://lh3.googleusercontent.com/-4WwoScStKhE/AAAAAAAAAAI/AAAAAAAAAAA/AMZuucmXa2ura4-mBTV7QUIpG0FoS8iWzw/s96-c/photo.jpg','en','2021-03-21 12:37:01','https://virtualchief.eu.auth0.com/','637519233962817331.N2Q2MjVkNjQtMTNhNS00ZmUxLTlmZTItMTJhMDYwOWNjOTE4OTFkNWNjMWMtMDgwOC00MWFjLWI2YmEtYjhmMTc1MmM1NDYw','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJnaXZlbl9uYW1lIjoiTWF0dGVvIiwiZmFtaWx5X25hbWUiOiJHcmlzbyIsIm5pY2tuYW1lIjoibWdyaXNvIiwibmFtZSI6Ik1hdHRlbyBHcmlzbyIsInBpY3R1cmUiOiJodHRwczovL2xoMy5nb29nbGV1c2VyY29udGVudC5jb20vLTRXd29TY1N0S2hFL0FBQUFBQUFBQUFJL0FBQUFBQUFBQUFBL0FNWnV1Y21YYTJ1cmE0LW1CVFY3UVVJcEcwRm9TOGlXencvczk2LWMvcGhvdG8uanBnIiwibG9jYWxlIjoiZW4iLCJ1cGRhdGVkX2F0IjoiMjAyMS0wMy0yMVQxMTozNzowMS4xMTBaIiwiZW1haWwiOiJtZ3Jpc29Aa2FpemVua2V5LmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJpc3MiOiJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vIiwic3ViIjoiZ29vZ2xlLW9hdXRoMnwxMTA5OTU1NjU5MzE4MTU0OTM0MjYiLCJhdWQiOiIxaEJnaFgzWXEweEtNODZFcmtHNG42WHVydlI1U0c1MiIsImlhdCI6MTYxNjMyNjYyMSwiZXhwIjoxNjE2MzYyNjIxLCJhdF9oYXNoIjoiS1pyejJhYklvVk52VEdRX0R3QmE4ZyIsImNfaGFzaCI6IlNJYVlNU252eGZCZFN6TmtjWE8xMGciLCJub25jZSI6IjYzNzUxOTIzMzk2MjgxNzMzMS5OMlEyTWpWa05qUXRNVE5oTlMwMFptVXhMVGxtWlRJdE1USmhNRFl3T1dOak9URTRPVEZrTldOak1XTXRNRGd3T0MwME1XRmpMV0kyWW1FdFlqaG1NVGMxTW1NMU5EWXcifQ.SZmkzFD7BBQXmveynLLwUwElXIcF8QpzfxR_eHiKyu6YI_vMg0_8klJrpse7c5E2KjOC0CKIo7LOQQS_WKyHcOnKVpYFtLmIMa3k5G7miPXzlarobVHKfpUTN76NegeERtBWNkXQCTqrwGk6Stco-w-qhEmw4RlXUVotTk0eF4249dI8JXQc_u53PWRAML1bwrbaTT2F_V2RcJdEl5q06N0ciDYDohAl0xeh5oGzR2BLIbAdrBa7QRLpM_Ok3lXrubAJvxp7s7Qx52eqA4e373qGmfmaApaLRQgg1BhkDFvNs_WJV_O5508eu7xmxN2YY4cQZuCXqN6q-r-tNg6agw','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJpc3MiOiJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vIiwic3ViIjoiZ29vZ2xlLW9hdXRoMnwxMTA5OTU1NjU5MzE4MTU0OTM0MjYiLCJhdWQiOlsiaHR0cHM6Ly92aXJ0dWFsY2hpZWYuZXUuYXV0aDAuY29tL2FwaS92Mi8iLCJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vdXNlcmluZm8iXSwiaWF0IjoxNjE2MzI2NjIxLCJleHAiOjE2MTYzMzM4MjEsImF6cCI6IjFoQmdoWDNZcTB4S004NkVya0c0bjZYdXJ2UjVTRzUyIiwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCJ9.mA7cQm2TrFFoO3C2aFhosrmgAIfEvH4vhyIpiCTceI50gmwyK8Xi5qOKAVw6ORqviomNUc1ireFlZgVT0fhV7smrh80pd8Hm6m21lRF1SVKqOcbK-dvmofJny2q1cenf4Ouu1IfakGqgHU0xXZ1pMjgEOVqqTxVUs_uV0A4ANZSf_WUF4SAbNQn3X6kS7DI6kbec7UohCKFQ8QhA4DCdrVg5SPyvXXVv9LgtHMYfJrdaz-4_GzqCCmuIDJi5T7ba0lfmXO7JgnmcbfgLTPLa0LyQ4jbfKsqZvvATXxBb774w5rtnFU7J9mF-n916nKgUslebyPMD5vgE2nUmutWIeQ',NULL,'2021-03-21 11:37:02','2021-08-04 06:55:46','\0'),(14,'google-oauth2|102438461008748600743','matteo.griso@virtualchief.net','Matteo','Griso','matteo.griso','https://lh3.googleusercontent.com/a-/AOh14GiqxYyXYkOCqppmKiVyRT8LweAijKFY55NfhsAFQKY9Xai1zEhkLe7zQfVMph_yWZpRLVMeK_GBrQQdGGn25ESU6-SPM0I_ojV9G7-4-9kgI7vzLHw7RXlZs_jAwg-eWODN_Zq2Z-dH7r564XBI2Pw0Lv1pFaexwlNTDIAUP3ijpJkNWTKW-zi7CjFmacZFfWDNjaeyWuyLNe8eNi6s-zgmIbTOgvFg7e1XJhwcDnKYYrMWZYUbfhzJpOQRu4qgcZYrDySzZpMI4PzcwttMcN2x8KCV-cr58-ZIadC-MkF9pV-dq9lz2mXisVP-HgmYyzyISeFJrOFEyPHblcUDPrJPkTs94AkVeXqZ4UBrCCWS51W9m3ol4E76HnkJhL_0eiThO1rCb6u1Swa9Ncad2E_4YU6wRRUmwWWOaBlsQoZu6-2VENuOYckqNnkOvgNT2Rbmr7rkX4ymZHEc11tjKuWreLwnfwzxeedsToUwIsxPNl2JVcio5NRc5-WbgS6jah43MlATt6iTDA6bswgTuY9D4NE43jTvgI8EEEGhiHuC9nwYCmeYLgJGbbE8Bcb1wz8nByT1h7b1_Vxl1pog1rO4ICGJFK5z7F_oBjETH9XbTyDbrJLBFHIEEWW7dSKtt5fsLWGRBrWJNvqqYgMaTjnx2YXZE_Iif4DoOZJLq0MF-k16d_WXSP64oc9qPFw748MI0GpG23Bhso5TfawKQjs4YpWLNw0DTjC3rdwW7zMg-hdrZhy0J_6HivS-Gl3ygjoMMQ=s96-c','en','2021-05-15 19:36:34','https://virtualchief.eu.auth0.com/','637566969664713421.MTg5YWRmY2YtNmVlOS00ZDBmLTlkYzYtOTNkMmNlZmFhNzExOGNhNjRlNGQtMWRhZi00MDk5LTkwNjgtMzE2NzhlMmVlMzYz','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJnaXZlbl9uYW1lIjoiTWF0dGVvIiwiZmFtaWx5X25hbWUiOiJHcmlzbyIsIm5pY2tuYW1lIjoibWF0dGVvLmdyaXNvIiwibmFtZSI6Ik1hdHRlbyBHcmlzbyIsInBpY3R1cmUiOiJodHRwczovL2xoMy5nb29nbGV1c2VyY29udGVudC5jb20vYS0vQU9oMTRHaXF4WXlYWWtPQ3FwcG1LaVZ5UlQ4THdlQWlqS0ZZNTVOZmhzQUZRS1k5WGFpMXpFaGtMZTd6UWZWTXBoX3lXWnBSTFZNZUtfR0JyUVFkR0duMjVFU1U2LVNQTTBJX29qVjlHNy00LTlrZ0k3dnpMSHc3UlhsWnNfakF3Zy1lV09ETl9acTJaLWRIN3I1NjRYQkkyUHcwTHYxcEZhZXh3bE5URElBVVAzaWpwSmtOV1RLVy16aTdDakZtYWNaRmZXRE5qYWV5V3V5TE5lOGVOaTZzLXpnbUliVE9ndkZnN2UxWEpod2NEbktZWXJNV1pZVWJmaHpKcE9RUnU0cWdjWllyRHlTelpwTUk0UHpjd3R0TWNOMng4S0NWLWNyNTgtWklhZEMtTWtGOXBWLWRxOWx6Mm1YaXNWUC1IZ21ZeXp5SVNlRkpyT0ZFeVBIYmxjVURQckpQa1RzOTRBa1ZlWHFaNFVCckNDV1M1MVc5bTNvbDRFNzZIbmtKaExfMGVpVGhPMXJDYjZ1MVN3YTlOY2FkMkVfNFlVNndSUlVtd1dXT2FCbHNRb1p1Ni0yVkVOdU9ZY2txTm5rT3ZnTlQyUmJtcjdya1g0eW1aSEVjMTF0akt1V3JlTHduZnd6eGVlZHNUb1V3SXN4UE5sMkpWY2lvNU5SYzUtV2JnUzZqYWg0M01sQVR0NmlUREE2YnN3Z1R1WTlENE5FNDNqVHZnSThFRUVHaGlIdUM5bndZQ21lWUxnSkdiYkU4QmNiMXd6OG5CeVQxaDdiMV9WeGwxcG9nMXJPNElDR0pGSzV6N0Zfb0JqRVRIOVhiVHlEYnJKTEJGSElFRVdXN2RTS3R0NWZzTFdHUkJyV0pOdnFxWWdNYVRqbngyWVhaRV9JaWY0RG9PWkpMcTBNRi1rMTZkX1dYU1A2NG9jOXFQRnc3NDhNSTBHcEcyM0Joc281VGZhd0tRanM0WXBXTE53MERUakMzcmR3Vzd6TWctaGRyWmh5MEpfNkhpdlMtR2wzeWdqb01NUT1zOTYtYyIsImxvY2FsZSI6ImVuIiwidXBkYXRlZF9hdCI6IjIwMjEtMDUtMTVUMTc6MzY6MzQuMzA3WiIsImVtYWlsIjoibWF0dGVvLmdyaXNvQHZpcnR1YWxjaGllZi5uZXQiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly92aXJ0dWFsY2hpZWYuZXUuYXV0aDAuY29tLyIsInN1YiI6Imdvb2dsZS1vYXV0aDJ8MTAyNDM4NDYxMDA4NzQ4NjAwNzQzIiwiYXVkIjoiMWhCZ2hYM1lxMHhLTTg2RXJrRzRuNlh1cnZSNVNHNTIiLCJpYXQiOjE2MjExMDAxOTQsImV4cCI6MTYyMTEzNjE5NCwiYXRfaGFzaCI6IjhOd1N2eWRzaFV3cWY2d05VbEdlcVEiLCJjX2hhc2giOiJsUXpvbExLYWxBTGlKSVgzS1JFX2x3Iiwibm9uY2UiOiI2Mzc1NjY5Njk2NjQ3MTM0MjEuTVRnNVlXUm1ZMll0Tm1WbE9TMDBaREJtTFRsa1l6WXRPVE5rTW1ObFptRmhOekV4T0dOaE5qUmxOR1F0TVdSaFppMDBNRGs1TFRrd05qZ3RNekUyTnpobE1tVmxNell6In0.O-cwc_YFTBIBiLaAdzueh6vyQVDk9JwcB9ZfffBxPYtGeRdx5OWisnuo1-17FyxOCE64k3Vpkt9mtBndS3lvubRPBAmCJ_DFVbSS7Ca9kf-yUAmWuN6zXqXaYzn3zr6t6yp3PVHTDZU3ruOC_WJ55rI-DkDcVaQytAQLJMsD8x7kMgcP6E9KBRBNsDHhLJ_KmPlxIi250CU4140TqZOqefNQZYfBPwMpLamJKbYotrnCJwJtEjpes4Vwrhie7pkfc7oFurobKRc4leQRf7rovaMKyvCEfITBU0vlRSlUqf-429vWpvrUhvfB2A5RVehKzl5yBZDZsC5PM9ZOMc02nA','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJpc3MiOiJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vIiwic3ViIjoiZ29vZ2xlLW9hdXRoMnwxMDI0Mzg0NjEwMDg3NDg2MDA3NDMiLCJhdWQiOlsiaHR0cHM6Ly92aXJ0dWFsY2hpZWYuZXUuYXV0aDAuY29tL2FwaS92Mi8iLCJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vdXNlcmluZm8iXSwiaWF0IjoxNjIxMTAwMTk0LCJleHAiOjE2MjExMDczOTQsImF6cCI6IjFoQmdoWDNZcTB4S004NkVya0c0bjZYdXJ2UjVTRzUyIiwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCJ9.I-5lQc0hPPO9R1LUzYKhVapxvDVunGve-EJIBTjT8IcLgUJG2OFJbpNFAvHllfykkCcSQzvE0oxCxWqeRWBbk4R5833Fqi5GpRNoJrgt3rwdBI8vhlfOfhVpFCNl-YkDDSACGFNgIEJMosVo5a8_8_NCpULQZeGk1URtVHyQoCbYID_gcujhu-mrg5KV4OMCk8gBLN0ZnmB8gFMnsqMnHPgq9LBi3eZJ5veTmpdNI9_qAhI3szDeeabqt3-SC-xUGT1AsIayJjG3kK-OfPkeLhSoQ1SCpoiiTuJEfcCRuesc2j9VmQf4ERn2TIKIua3y43Ck7EU_11o29urE7vwsdg',NULL,'2021-05-15 17:39:26','2021-05-16 17:03:17','\0'),(15,'auth0|60f29a0921c3820069d583ba','m.griso@hotmail.it',NULL,NULL,'m.griso','https://s.gravatar.com/avatar/00265c673015126cb517747241e3f4ba?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fm.png',NULL,'2021-07-17 10:51:22','https://virtualchief.eu.auth0.com/','637621086747297983.ZDIyM2NlNjktMTVjZC00YmY3LThjZjAtYjUyYWE2MWMxOWE1NDY0MWFkMTItZDQ2Ni00YjMxLTgyOWMtYzdmNWQxMDAwOTgw','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJuaWNrbmFtZSI6Im0uZ3Jpc28iLCJuYW1lIjoibS5ncmlzb0Bob3RtYWlsLml0IiwicGljdHVyZSI6Imh0dHBzOi8vcy5ncmF2YXRhci5jb20vYXZhdGFyLzAwMjY1YzY3MzAxNTEyNmNiNTE3NzQ3MjQxZTNmNGJhP3M9NDgwJnI9cGcmZD1odHRwcyUzQSUyRiUyRmNkbi5hdXRoMC5jb20lMkZhdmF0YXJzJTJGbS5wbmciLCJ1cGRhdGVkX2F0IjoiMjAyMS0wNy0xN1QwODo1MToyMi4yMTZaIiwiZW1haWwiOiJtLmdyaXNvQGhvdG1haWwuaXQiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImlzcyI6Imh0dHBzOi8vdmlydHVhbGNoaWVmLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJhdXRoMHw2MGYyOWEwOTIxYzM4MjAwNjlkNTgzYmEiLCJhdWQiOiIxaEJnaFgzWXEweEtNODZFcmtHNG42WHVydlI1U0c1MiIsImlhdCI6MTYyNjUxMTg4NiwiZXhwIjoxNjI2NTQ3ODg2LCJhdF9oYXNoIjoiS3BWX3BkVTlrSFN5bDRZU3d3Y1FfZyIsImNfaGFzaCI6IjZLWVhKLWNfQWItV0pudk5BWWVfd3ciLCJub25jZSI6IjYzNzYyMTA4Njc0NzI5Nzk4My5aREl5TTJObE5qa3RNVFZqWkMwMFltWTNMVGhqWmpBdFlqVXlZV0UyTVdNeE9XRTFORFkwTVdGa01USXRaRFEyTmkwMFlqTXhMVGd5T1dNdFl6ZG1OV1F4TURBd09UZ3cifQ.gKDAy5UGLhjuoB5F86yeI5rbtyfKYF3cS_JnoRk7Yv9GBv5IXHJmMn9EiC6SuVQNNwSTY4VjgBbgA5J3TXKG28adbzDt2yUM92Q1pGNzLmQrIQrNdKW88AhNjdEgqfm1d56cDqmGfpMQLBVGUruFUMJBKyFV6iSfIZtn1hS9mbmDlCpTgenVmL7F0xWhDbyfgJrKfCr7eSCnBd86-iNy5wJFravQy2veCQrPsu8T3CfhM1vO8JCaahxU-htbyYfcA7TBjkvQxxmnIrH-UZpbP3Dic6BhHwohVDIwIp7tTTxtbWRY5k4eP674rp9hxB14em3prDC9fd7ZxmFsMQ-S6A','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJpc3MiOiJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vIiwic3ViIjoiYXV0aDB8NjBmMjlhMDkyMWMzODIwMDY5ZDU4M2JhIiwiYXVkIjpbImh0dHBzOi8vdmlydHVhbGNoaWVmLmV1LmF1dGgwLmNvbS9hcGkvdjIvIiwiaHR0cHM6Ly92aXJ0dWFsY2hpZWYuZXUuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTYyNjUxMTg4NiwiZXhwIjoxNjI2NTE5MDg2LCJhenAiOiIxaEJnaFgzWXEweEtNODZFcmtHNG42WHVydlI1U0c1MiIsInNjb3BlIjoib3BlbmlkIHByb2ZpbGUgZW1haWwifQ.obwvdJ9jCIodZuYzqdGZVOjIe2E4NPypnuaUMLdgetExfKx6Fe-UA8Fhl9VVM8iMpoRb_A40d60u9ASA4IhiJ4rFj6xN5fY1fWXbs0c58VgveKLQIVrWltfW0r7CY41Td5evd6ucdwwU3xNA4NYMP6GGTmVNsAFHG5J8_W_vTOXuOfskyrWh6WSZPccQaOI0ZD0xwHOfPAo6dVsbgoNXQ8pO4fd-nOsFnVQYwngtrgL-2rEkwi_m6p3PxrBDWF_fQ9ytWFGsQm2wHW_6yGqzqg0UVRL-3cgzGmtYLwHNtfBedpWtHhljeevuxnLFJrvf3QYMUmIGoI1azjLJD939DQ',NULL,'2021-07-17 08:51:27','2021-07-17 09:24:38','\0'),(16,'auth0|6113c03728db210071158bdf','mgrisovr@gmail.com',NULL,NULL,'mgrisovr','https://s.gravatar.com/avatar/fc33af062e3f6b53370aa10433cdbeff?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fmg.png',NULL,'2021-08-11 14:19:04','https://virtualchief.eu.auth0.com/','637642811263481718.OTgzMzY0NzgtYjQwYS00MmRjLWI4MWItYjAyYzE2ZjYxYmU4NDI4NzUyZTItYzQwZS00YzYzLWJhNTQtODMxOTljNzVlODg1','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJuaWNrbmFtZSI6Im1ncmlzb3ZyIiwibmFtZSI6Im1ncmlzb3ZyQGdtYWlsLmNvbSIsInBpY3R1cmUiOiJodHRwczovL3MuZ3JhdmF0YXIuY29tL2F2YXRhci9mYzMzYWYwNjJlM2Y2YjUzMzcwYWExMDQzM2NkYmVmZj9zPTQ4MCZyPXBnJmQ9aHR0cHMlM0ElMkYlMkZjZG4uYXV0aDAuY29tJTJGYXZhdGFycyUyRm1nLnBuZyIsInVwZGF0ZWRfYXQiOiIyMDIxLTA4LTExVDEyOjE5OjA0LjEwN1oiLCJlbWFpbCI6Im1ncmlzb3ZyQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiaXNzIjoiaHR0cHM6Ly92aXJ0dWFsY2hpZWYuZXUuYXV0aDAuY29tLyIsInN1YiI6ImF1dGgwfDYxMTNjMDM3MjhkYjIxMDA3MTE1OGJkZiIsImF1ZCI6IjFoQmdoWDNZcTB4S004NkVya0c0bjZYdXJ2UjVTRzUyIiwiaWF0IjoxNjI4Njg0MzQ4LCJleHAiOjE2Mjg3MjAzNDgsImF0X2hhc2giOiJVSXZDRk9PbG5ySmhoTU10RmIzNHpBIiwiY19oYXNoIjoiUkh5cnhqWjhnSU90dlE3TXNCdWxxQSIsIm5vbmNlIjoiNjM3NjQyODExMjYzNDgxNzE4Lk9UZ3pNelkwTnpndFlqUXdZUzAwTW1SakxXSTRNV0l0WWpBeVl6RTJaall4WW1VNE5ESTROelV5WlRJdFl6UXdaUzAwWXpZekxXSmhOVFF0T0RNeE9UbGpOelZsT0RnMSJ9.GQQXYSpH1QDHegmWD4fi_9leWJXyNeMKL7WNcAXCZ0FSei4-K5AykZ8smr-JfyM6KweXNub5fJ2-KuTtJSHiW0LbmVgmLLMamMVa-ZregKMMv62A2VJYBiwpi9JU2u0eGWnax4V-oqG6_sZhrayPqne-L_6XKzH3Hw0byjkPgh9_0Is11zlJKAiIZw6nsVJpXvjjm35y59gvmuAkRxcKcfThvADfdZszJcaYJ21kpGlz4bDDYBeDpiQAPX71PXPEWDNc5hCr9ooQzKdmjPJbIrYdjlHneLUs1I49Imk-JDPXZyy4XGv2MBUMStEbnO1DhnDzEAcuQbvhN4BhSpYaew','eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Inc5MkRHTnpuVUNFeWNRdmFNT0k4TyJ9.eyJpc3MiOiJodHRwczovL3ZpcnR1YWxjaGllZi5ldS5hdXRoMC5jb20vIiwic3ViIjoiYXV0aDB8NjExM2MwMzcyOGRiMjEwMDcxMTU4YmRmIiwiYXVkIjpbImh0dHBzOi8vdmlydHVhbGNoaWVmLmV1LmF1dGgwLmNvbS9hcGkvdjIvIiwiaHR0cHM6Ly92aXJ0dWFsY2hpZWYuZXUuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTYyODY4NDM0OCwiZXhwIjoxNjI4NjkxNTQ4LCJhenAiOiIxaEJnaFgzWXEweEtNODZFcmtHNG42WHVydlI1U0c1MiIsInNjb3BlIjoib3BlbmlkIHByb2ZpbGUgZW1haWwifQ.gUtch7Kflr86WoxmgZ4vltwBOe9VzXEsZE4sDcCVQfsC5SH6LXBaAJUrOsQzxUfx9ey44-98GijQfU6POCYSlAN5N6L_Qj0dXIw1t4kS6fzXFYQc2UUaQ6vVDz_RGXABsXY9mRrfEmacBjI4UhtOP5TOHO-qzOITByV98drmP-1qgchuCItx2B5hxSqSsOHxDG_2wRNmqICkUqPspiuw6kv0ZtWCODYbqipuxgidxj7xPqQV8GJ4qUX1p4pfyiUbbES4rM6Bthdwpj31LTGlSgqQY4B_IX6B9Yhr8Tk07-wP99lBFYwFikZH_pxcbSQFW2Obplx8nNyJm4_g4zeODg',NULL,'2021-08-11 12:19:11','2021-08-11 14:43:07','\0');
/*!40000 ALTER TABLE `useraccounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `useraccountsgroups`
--

DROP TABLE IF EXISTS `useraccountsgroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `useraccountsgroups` (
  `groupid` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  `workspaceid` int(11) NOT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`groupid`,`userid`,`workspaceid`),
  KEY `workspace_FK2_idx` (`workspaceid`),
  KEY `userif_FK2_idx` (`userid`),
  CONSTRAINT `group_FK2` FOREIGN KEY (`groupid`) REFERENCES `groupss` (`id`),
  CONSTRAINT `userif_FK2` FOREIGN KEY (`userid`) REFERENCES `useraccounts` (`id`),
  CONSTRAINT `workspace_FK2` FOREIGN KEY (`workspaceid`) REFERENCES `workspaces` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useraccountsgroups`
--

LOCK TABLES `useraccountsgroups` WRITE;
/*!40000 ALTER TABLE `useraccountsgroups` DISABLE KEYS */;
INSERT INTO `useraccountsgroups` VALUES (2,13,21,'2021-07-12 22:13:54'),(3,14,21,'2021-07-17 10:29:32'),(4,7,23,'2021-08-01 16:52:42'),(4,13,21,'2021-06-27 00:00:00'),(5,7,23,'2021-08-02 15:05:36'),(5,13,21,'2021-07-20 22:27:08'),(13,7,22,'2021-05-30 00:00:00'),(13,7,23,'2021-07-21 21:49:12'),(13,13,21,'2021-03-21 12:55:47'),(14,7,21,'2021-07-17 11:26:54'),(14,13,22,'2021-05-30 18:37:07'),(14,13,23,'2021-07-21 22:01:44'),(14,14,21,'2021-05-16 19:03:38'),(14,15,21,'2021-07-17 11:15:59'),(14,16,23,'2021-08-11 14:27:02');
/*!40000 ALTER TABLE `useraccountsgroups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `useraccountworkspaces`
--

DROP TABLE IF EXISTS `useraccountworkspaces`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `useraccountworkspaces` (
  `userid` int(11) NOT NULL,
  `workspaceid` int(11) NOT NULL,
  `invite_sent` bit(1) DEFAULT b'0',
  `invite_sent_date` datetime DEFAULT NULL,
  `invite_checksum` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  `invitation_accepted` bit(1) DEFAULT b'0',
  `invitation_accepted_date` datetime DEFAULT NULL,
  `default_ws` bit(1) DEFAULT b'0',
  `destinationUrl` varchar(255) COLLATE utf32_bin DEFAULT NULL,
  PRIMARY KEY (`userid`,`workspaceid`),
  KEY `workspaceid_FK1_idx` (`workspaceid`),
  CONSTRAINT `userid_FIK1` FOREIGN KEY (`userid`) REFERENCES `useraccounts` (`id`),
  CONSTRAINT `workspaceid_FK1` FOREIGN KEY (`workspaceid`) REFERENCES `workspaces` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useraccountworkspaces`
--

LOCK TABLES `useraccountworkspaces` WRITE;
/*!40000 ALTER TABLE `useraccountworkspaces` DISABLE KEYS */;
INSERT INTO `useraccountworkspaces` VALUES (7,21,'','2021-07-17 10:30:29',NULL,'','2021-07-17 09:26:54','\0',''),(7,22,NULL,NULL,'ETaNBd2yviWalVgM','\0',NULL,'',NULL),(7,23,NULL,NULL,'YUaYG4KbiRcoiRu6','\0',NULL,'',NULL),(13,21,NULL,NULL,NULL,'\0',NULL,'',NULL),(13,22,'','2021-05-30 18:35:42',NULL,'','2021-05-30 16:37:07','\0',''),(13,23,'','2021-07-21 21:59:37',NULL,'','2021-07-21 20:01:44','\0',''),(14,21,'','2021-05-15 18:13:45',NULL,'','2021-05-16 17:03:38','\0',''),(15,21,'','2021-07-17 10:30:10',NULL,'','2021-07-17 09:15:59','\0',''),(16,23,'','2021-08-11 14:21:39',NULL,'','2021-08-11 12:27:02','\0','');
/*!40000 ALTER TABLE `useraccountworkspaces` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `workspaces`
--

DROP TABLE IF EXISTS `workspaces`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `workspaces` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf32_bin NOT NULL,
  `creationdate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creator` varchar(255) COLLATE utf32_bin NOT NULL,
  `enabled` bit(1) NOT NULL,
  `enableddate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workspaces`
--

LOCK TABLES `workspaces` WRITE;
/*!40000 ALTER TABLE `workspaces` DISABLE KEYS */;
INSERT INTO `workspaces` VALUES (21,'kaizenkey','2021-03-21 11:55:47','google-oauth2|110995565931815493426','','2021-03-21 11:55:47'),(22,'matteo','2021-04-29 20:04:05','google-oauth2|106708164772575417205','','2021-04-29 20:04:05'),(23,'Testws','2021-07-21 19:49:12','google-oauth2|106708164772575417205','','2021-07-21 19:49:12');
/*!40000 ALTER TABLE `workspaces` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `workspacesinvites`
--

DROP TABLE IF EXISTS `workspacesinvites`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `workspacesinvites` (
  `mail` varchar(255) COLLATE utf32_bin NOT NULL,
  `workspace` int(11) NOT NULL,
  `invited_by` int(11) NOT NULL,
  `checksum` varchar(24) COLLATE utf32_bin NOT NULL,
  `sent_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `accepted` bit(1) DEFAULT b'0',
  `accepted_date` datetime DEFAULT NULL,
  PRIMARY KEY (`mail`,`workspace`,`sent_date`),
  KEY `ws_FK1_idx` (`workspace`),
  KEY `usracc_FK1_idx` (`invited_by`),
  CONSTRAINT `usracc_FK1` FOREIGN KEY (`invited_by`) REFERENCES `useraccounts` (`id`),
  CONSTRAINT `ws_FK1` FOREIGN KEY (`workspace`) REFERENCES `workspaces` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workspacesinvites`
--

LOCK TABLES `workspacesinvites` WRITE;
/*!40000 ALTER TABLE `workspacesinvites` DISABLE KEYS */;
INSERT INTO `workspacesinvites` VALUES ('m.griso@hotmail.it',21,13,'795DAxQG1pGbQx7X','2021-07-17 10:30:10','','2021-07-17 11:15:59'),('matteo.griso@virtualchief.net',21,13,'X3w0fXfUKo15xnZo','2021-05-15 18:13:45','','2021-05-16 19:03:38'),('mgriso@kaizenkey.com',22,7,'jDl72ly0ikxV5apl','2021-05-01 18:30:24','\0',NULL),('mgriso@kaizenkey.com',22,7,'PI8lySZd42RxuSU2','2021-05-30 18:35:42','','2021-05-30 18:37:07'),('mgriso@kaizenkey.com',23,7,'HhI11Za0eCKhuPDh','2021-07-21 21:59:37','','2021-07-21 22:01:44'),('mgrisoster@gmail.com',21,13,'n0ga79JCBLBwLCyP','2021-07-17 10:30:29','','2021-07-17 11:26:54'),('mgrisovr@gmail.com',23,7,'oiXzvOfwld0z9xuR','2021-08-11 14:21:05','','2021-08-11 14:27:02'),('mgrisovr@gmail.com',23,7,'atzNEBVkH6RZ6cMy','2021-08-11 14:21:35','','2021-08-11 14:27:02'),('mgrisovr@gmail.com',23,7,'dMjT4jIC95bmnZOD','2021-08-11 14:21:37','','2021-08-11 14:27:02'),('mgrisovr@gmail.com',23,7,'50M83LdTujOo4pvX','2021-08-11 14:21:39','','2021-08-11 14:27:02');
/*!40000 ALTER TABLE `workspacesinvites` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-10-17 12:09:18
