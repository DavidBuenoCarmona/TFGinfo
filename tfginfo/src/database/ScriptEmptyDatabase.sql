CREATE DATABASE  IF NOT EXISTS `tfginfo` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `tfginfo`;
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
-- Table structure for table `career`
--

DROP TABLE IF EXISTS `career`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `career` (
  `id` int NOT NULL AUTO_INCREMENT,
  `university` int NOT NULL,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `University_career_idx` (`university`),
  CONSTRAINT `University_career` FOREIGN KEY (`university`) REFERENCES `university` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `career`
--

LOCK TABLES `career` WRITE;
/*!40000 ALTER TABLE `career` DISABLE KEYS */;
/*!40000 ALTER TABLE `career` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `department`
--

DROP TABLE IF EXISTS `department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `department` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `acronym` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `department`
--

LOCK TABLES `department` WRITE;
/*!40000 ALTER TABLE `department` DISABLE KEYS */;
/*!40000 ALTER TABLE `department` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `professor`
--

DROP TABLE IF EXISTS `professor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `professor` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  `surname` varchar(45) NOT NULL,
  `department` int NOT NULL,
  `department_boss` smallint DEFAULT NULL,
  `user` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Department_professor_idx` (`department`),
  KEY `user_professor_idx` (`user`),
  CONSTRAINT `Department_professor` FOREIGN KEY (`department`) REFERENCES `department` (`id`),
  CONSTRAINT `user_professor` FOREIGN KEY (`user`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `professor`
--

LOCK TABLES `professor` WRITE;
/*!40000 ALTER TABLE `professor` DISABLE KEYS */;
/*!40000 ALTER TABLE `professor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `code` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'Admin','%ADMIN'),(2,'Professor','%PROF'),(3,'Student','%STUD');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  `surname` varchar(45) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `career` int NOT NULL,
  `phone` varchar(45) DEFAULT NULL,
  `address` varchar(45) DEFAULT NULL,
  `birthdate` datetime DEFAULT NULL,
  `user` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `dni_UNIQUE` (`dni`),
  KEY `User_Student_idx` (`user`),
  KEY `Career_Student_idx` (`career`),
  CONSTRAINT `Career_Student` FOREIGN KEY (`career`) REFERENCES `career` (`id`),
  CONSTRAINT `User_Student` FOREIGN KEY (`user`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tfg`
--

DROP TABLE IF EXISTS `tfg`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg` (
  `id` int NOT NULL AUTO_INCREMENT,
  `tfg_line` int NOT NULL,
  `startdate` datetime NOT NULL,
  `external_tutor_email` varchar(45) DEFAULT NULL,
  `external_tutor_name` varchar(45) DEFAULT NULL,
  `accepted` smallint DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `tfg_line_idx` (`tfg_line`),
  CONSTRAINT `tfg_line` FOREIGN KEY (`tfg_line`) REFERENCES `tfg_line` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tfg`
--

LOCK TABLES `tfg` WRITE;
/*!40000 ALTER TABLE `tfg` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tfg_line`
--

DROP TABLE IF EXISTS `tfg_line`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg_line` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `description` text NOT NULL,
  `slots` int NOT NULL,
  `group` smallint NOT NULL,
  `department` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `tfg_line_department_idx` (`department`),
  CONSTRAINT `tfg_line_department` FOREIGN KEY (`department`) REFERENCES `department` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tfg_line`
--

LOCK TABLES `tfg_line` WRITE;
/*!40000 ALTER TABLE `tfg_line` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_line` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tfg_line_career`
--

DROP TABLE IF EXISTS `tfg_line_career`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg_line_career` (
  `id` int NOT NULL AUTO_INCREMENT,
  `tfg_line` int NOT NULL,
  `career` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Career_TFG_idx` (`career`),
  KEY `TFG_Career_idx` (`tfg_line`),
  CONSTRAINT `Career_TFG` FOREIGN KEY (`career`) REFERENCES `career` (`id`),
  CONSTRAINT `TFG_Career` FOREIGN KEY (`tfg_line`) REFERENCES `tfg_line` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tfg_line_career`
--

LOCK TABLES `tfg_line_career` WRITE;
/*!40000 ALTER TABLE `tfg_line_career` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_line_career` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tfg_line_professor`
--

DROP TABLE IF EXISTS `tfg_line_professor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg_line_professor` (
  `id` int NOT NULL AUTO_INCREMENT,
  `tfg_line` int NOT NULL,
  `professor` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `tfg_line_professor_idx` (`tfg_line`),
  KEY `professor_tfg_line_idx` (`professor`),
  CONSTRAINT `professor_tfg_line` FOREIGN KEY (`professor`) REFERENCES `professor` (`id`),
  CONSTRAINT `tfg_line_professor` FOREIGN KEY (`tfg_line`) REFERENCES `tfg_line` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tfg_line_professor`
--

LOCK TABLES `tfg_line_professor` WRITE;
/*!40000 ALTER TABLE `tfg_line_professor` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_line_professor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tfg_professor`
--

DROP TABLE IF EXISTS `tfg_professor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg_professor` (
  `id` int NOT NULL AUTO_INCREMENT,
  `professor` int NOT NULL,
  `tfg` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `tfg_professor_idx` (`professor`),
  KEY `professor_tfg_idx` (`tfg`),
  CONSTRAINT `professor_tfg` FOREIGN KEY (`tfg`) REFERENCES `tfg` (`id`),
  CONSTRAINT `tfg_professor` FOREIGN KEY (`professor`) REFERENCES `professor` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tfg_professor`
--

LOCK TABLES `tfg_professor` WRITE;
/*!40000 ALTER TABLE `tfg_professor` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_professor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tfg_student`
--

DROP TABLE IF EXISTS `tfg_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg_student` (
  `id` int NOT NULL AUTO_INCREMENT,
  `student` int NOT NULL,
  `tfg` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `student_idx` (`student`),
  KEY `tfg_student_idx` (`tfg`),
  CONSTRAINT `student` FOREIGN KEY (`student`) REFERENCES `student` (`id`),
  CONSTRAINT `tfg_student` FOREIGN KEY (`tfg`) REFERENCES `tfg` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tfg_student`
--

LOCK TABLES `tfg_student` WRITE;
/*!40000 ALTER TABLE `tfg_student` DISABLE KEYS */;
/*!40000 ALTER TABLE `tfg_student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `university`
--

DROP TABLE IF EXISTS `university`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `university` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `address` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `university`
--

LOCK TABLES `university` WRITE;
/*!40000 ALTER TABLE `university` DISABLE KEYS */;
INSERT INTO `university` VALUES (14,'ETSII','C/etsii N2');
/*!40000 ALTER TABLE `university` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `university_department`
--

DROP TABLE IF EXISTS `university_department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `university_department` (
  `id` int NOT NULL AUTO_INCREMENT,
  `university` int NOT NULL,
  `department` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `university_department_idx` (`university`),
  KEY `department_university_idx` (`department`),
  CONSTRAINT `department_university` FOREIGN KEY (`department`) REFERENCES `department` (`id`),
  CONSTRAINT `university_department` FOREIGN KEY (`university`) REFERENCES `university` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `university_department`
--

LOCK TABLES `university_department` WRITE;
/*!40000 ALTER TABLE `university_department` DISABLE KEYS */;
/*!40000 ALTER TABLE `university_department` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `password` varchar(45) DEFAULT NULL,
  `role` int NOT NULL,
  `auth_code` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username_UNIQUE` (`username`),
  KEY `role_user_idx` (`role`),
  CONSTRAINT `role_user` FOREIGN KEY (`role`) REFERENCES `role` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (34,'sa@gmail.com','TPaCmqk3KOjzyX35E/sb+pX+WBDikzoFlD+DEqmNnPI=',1,NULL);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `working_group`
--

DROP TABLE IF EXISTS `working_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `working_group` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `isPrivate` smallint NOT NULL,
  `description` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `working_group`
--

LOCK TABLES `working_group` WRITE;
/*!40000 ALTER TABLE `working_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `working_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `working_group_professor`
--

DROP TABLE IF EXISTS `working_group_professor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `working_group_professor` (
  `id` int NOT NULL AUTO_INCREMENT,
  `professor` int NOT NULL,
  `working_group` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Working_group_professor_idx` (`professor`),
  KEY `professor_working_group_idx` (`working_group`),
  CONSTRAINT `professor_working_group` FOREIGN KEY (`working_group`) REFERENCES `working_group` (`id`),
  CONSTRAINT `Working_group_professor` FOREIGN KEY (`professor`) REFERENCES `professor` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `working_group_professor`
--

LOCK TABLES `working_group_professor` WRITE;
/*!40000 ALTER TABLE `working_group_professor` DISABLE KEYS */;
/*!40000 ALTER TABLE `working_group_professor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `working_group_student`
--

DROP TABLE IF EXISTS `working_group_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `working_group_student` (
  `id` int NOT NULL AUTO_INCREMENT,
  `working_group` int NOT NULL,
  `student` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Working_group_student_idx` (`student`),
  KEY `student_working_group_idx` (`working_group`),
  CONSTRAINT `student_working_group` FOREIGN KEY (`working_group`) REFERENCES `working_group` (`id`),
  CONSTRAINT `Working_group_student` FOREIGN KEY (`student`) REFERENCES `student` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `working_group_student`
--

LOCK TABLES `working_group_student` WRITE;
/*!40000 ALTER TABLE `working_group_student` DISABLE KEYS */;
/*!40000 ALTER TABLE `working_group_student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `working_group_tfg`
--

DROP TABLE IF EXISTS `working_group_tfg`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `working_group_tfg` (
  `id` int NOT NULL AUTO_INCREMENT,
  `working_group` int NOT NULL,
  `tfg` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `working_group_tfg_idx` (`tfg`),
  KEY `tfg_working_group_idx` (`working_group`),
  CONSTRAINT `tfg_working_group` FOREIGN KEY (`working_group`) REFERENCES `working_group` (`id`),
  CONSTRAINT `working_group_tfg` FOREIGN KEY (`tfg`) REFERENCES `tfg` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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

-- Dump completed on 2025-06-25 11:44:59
