-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: tfginfo
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `career`
--

LOCK TABLES `career` WRITE;
/*!40000 ALTER TABLE `career` DISABLE KEYS */;
INSERT INTO `career` VALUES (2,2,'Software Super cool');
/*!40000 ALTER TABLE `career` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `department`
--

LOCK TABLES `department` WRITE;
/*!40000 ALTER TABLE `department` DISABLE KEYS */;
INSERT INTO `department` VALUES (3,'Robotica',3),(4,'Tech',2);
/*!40000 ALTER TABLE `department` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `external_tutor`
--

LOCK TABLES `external_tutor` WRITE;
/*!40000 ALTER TABLE `external_tutor` DISABLE KEYS */;
/*!40000 ALTER TABLE `external_tutor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `professor`
--

LOCK TABLES `professor` WRITE;
/*!40000 ALTER TABLE `professor` DISABLE KEYS */;
/*!40000 ALTER TABLE `professor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'Admin','%ADMIN'),(2,'Professor','%PROF'),(3,'Student','%STUD');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tfg`
--

LOCK TABLES `tfg` WRITE;
/*!40000 ALTER TABLE `tfg` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tfg_external_tutor`
--

LOCK TABLES `tfg_external_tutor` WRITE;
/*!40000 ALTER TABLE `tfg_external_tutor` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_external_tutor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tfg_line`
--

LOCK TABLES `tfg_line` WRITE;
/*!40000 ALTER TABLE `tfg_line` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_line` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tfg_line_career`
--

LOCK TABLES `tfg_line_career` WRITE;
/*!40000 ALTER TABLE `tfg_line_career` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_line_career` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tfg_professor`
--

LOCK TABLES `tfg_professor` WRITE;
/*!40000 ALTER TABLE `tfg_professor` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_professor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tfg_student`
--

LOCK TABLES `tfg_student` WRITE;
/*!40000 ALTER TABLE `tfg_student` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `university`
--

LOCK TABLES `university` WRITE;
/*!40000 ALTER TABLE `university` DISABLE KEYS */;
INSERT INTO `university` VALUES (2,'UPe','C/ Pepe lopez'),(3,'UMA','C/ Pepe lopez'),(5,'UPeos','C/ Pepe lopez'),(6,'UVAs','C/ Pepe lopez');
/*!40000 ALTER TABLE `university` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `working_group`
--

LOCK TABLES `working_group` WRITE;
/*!40000 ALTER TABLE `working_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `working_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `working_group_professor`
--

LOCK TABLES `working_group_professor` WRITE;
/*!40000 ALTER TABLE `working_group_professor` DISABLE KEYS */;
/*!40000 ALTER TABLE `working_group_professor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `working_group_student`
--

LOCK TABLES `working_group_student` WRITE;
/*!40000 ALTER TABLE `working_group_student` DISABLE KEYS */;
/*!40000 ALTER TABLE `working_group_student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `working_group_tfg`
--

LOCK TABLES `working_group_tfg` WRITE;
/*!40000 ALTER TABLE `working_group_tfg` DISABLE KEYS */;
/*!40000 ALTER TABLE `working_group_tfg` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-14 18:36:21
