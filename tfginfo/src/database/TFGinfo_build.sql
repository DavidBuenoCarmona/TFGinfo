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
  `id` int NOT NULL,
  `university` int NOT NULL,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `University_idx` (`university`),
  CONSTRAINT `University_career` FOREIGN KEY (`university`) REFERENCES `university` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `department`
--

DROP TABLE IF EXISTS `department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `department` (
  `id` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `university` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `University_department_idx` (`university`),
  CONSTRAINT `University_department` FOREIGN KEY (`university`) REFERENCES `university` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `professor`
--

DROP TABLE IF EXISTS `professor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `professor` (
  `id` int NOT NULL,
  `email` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  `surname` varchar(45) NOT NULL,
  `department` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Department_professor_idx` (`department`),
  CONSTRAINT `Department_professor` FOREIGN KEY (`department`) REFERENCES `department` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `id` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `code` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `birthdate` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `dni_UNIQUE` (`dni`),
  KEY `Career_Student_idx` (`career`),
  CONSTRAINT `Career_Student` FOREIGN KEY (`career`) REFERENCES `career` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tfg`
--

DROP TABLE IF EXISTS `tfg`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg` (
  `id` int NOT NULL,
  `tfg_line` int NOT NULL,
  `startdate` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `TFG_idx` (`tfg_line`),
  CONSTRAINT `TFG_LINE` FOREIGN KEY (`tfg_line`) REFERENCES `tfg_line` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tfg_line`
--

DROP TABLE IF EXISTS `tfg_line`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg_line` (
  `id` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `description` varchar(45) NOT NULL,
  `slots` int NOT NULL,
  `group` smallint NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tfg_line_career`
--

DROP TABLE IF EXISTS `tfg_line_career`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg_line_career` (
  `id` int NOT NULL,
  `tfg_line` int NOT NULL,
  `career` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `TFG_idx` (`tfg_line`),
  KEY `Career_idx` (`career`),
  CONSTRAINT `Career` FOREIGN KEY (`career`) REFERENCES `career` (`id`),
  CONSTRAINT `TFG` FOREIGN KEY (`tfg_line`) REFERENCES `tfg_line` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tfg_professor`
--

DROP TABLE IF EXISTS `tfg_professor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg_professor` (
  `id` int NOT NULL,
  `professor` int NOT NULL,
  `tfg` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `tfg_profesor_idx` (`tfg`),
  KEY `profesor_idx` (`professor`),
  CONSTRAINT `professor` FOREIGN KEY (`professor`) REFERENCES `professor` (`id`),
  CONSTRAINT `professor_tfg` FOREIGN KEY (`tfg`) REFERENCES `tfg` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tfg_student`
--

DROP TABLE IF EXISTS `tfg_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tfg_student` (
  `id` int NOT NULL,
  `student` int NOT NULL,
  `tfg` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `student_idx` (`student`),
  KEY `tfg_student_idx` (`tfg`),
  CONSTRAINT `student` FOREIGN KEY (`student`) REFERENCES `student` (`id`),
  CONSTRAINT `tfg_student` FOREIGN KEY (`tfg`) REFERENCES `tfg` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `university`
--

DROP TABLE IF EXISTS `university`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `university` (
  `id` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `address` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `working_group`
--

DROP TABLE IF EXISTS `working_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `working_group` (
  `id` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `private` smallint NOT NULL,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `working_group_professor`
--

DROP TABLE IF EXISTS `working_group_professor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `working_group_professor` (
  `id` int NOT NULL,
  `professor` int NOT NULL,
  `working_group` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Professor_Working_group_idx` (`working_group`),
  KEY `Working_group_professor_idx` (`professor`),
  CONSTRAINT `Professor_Working_group` FOREIGN KEY (`working_group`) REFERENCES `working_group` (`id`),
  CONSTRAINT `Working_group_professor` FOREIGN KEY (`professor`) REFERENCES `professor` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `working_group_student`
--

DROP TABLE IF EXISTS `working_group_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `working_group_student` (
  `id` int NOT NULL,
  `working_group` int NOT NULL,
  `student` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Working_group_idx` (`working_group`),
  KEY `Working_group_student_idx` (`student`),
  CONSTRAINT `Student_Working_group` FOREIGN KEY (`working_group`) REFERENCES `working_group` (`id`),
  CONSTRAINT `Working_group_student` FOREIGN KEY (`student`) REFERENCES `student` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `working_group_tfg`
--

DROP TABLE IF EXISTS `working_group_tfg`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `working_group_tfg` (
  `id` int NOT NULL,
  `working_group` int NOT NULL,
  `tfg` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Working_group_idx` (`working_group`),
  KEY `Working_group_tfg_idx` (`tfg`),
  CONSTRAINT `TFG_Working_group` FOREIGN KEY (`working_group`) REFERENCES `working_group` (`id`),
  CONSTRAINT `Working_group_tfg` FOREIGN KEY (`tfg`) REFERENCES `tfg` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-13 11:08:49
