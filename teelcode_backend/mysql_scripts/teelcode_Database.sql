-- MySQL dump 10.13  Distrib 8.0.43, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: teelcode
-- ------------------------------------------------------
-- Server version	8.0.43

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
-- Table structure for table `cosmetics`
--

DROP TABLE IF EXISTS `cosmetics`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cosmetics` (
  `cosmetic_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `level_required` int NOT NULL DEFAULT '1',
  `description` text,
  PRIMARY KEY (`cosmetic_id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cosmetics`
--

LOCK TABLES `cosmetics` WRITE;
/*!40000 ALTER TABLE `cosmetics` DISABLE KEYS */;
/*!40000 ALTER TABLE `cosmetics` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventory`
--

DROP TABLE IF EXISTS `inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventory` (
  `inv_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `cosmetic_id` int NOT NULL,
  `unlocked` tinyint(1) NOT NULL DEFAULT '0',
  `equipped` tinyint(1) NOT NULL DEFAULT '0',
  `unlocked_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`inv_id`),
  UNIQUE KEY `uq_user_cosmetic` (`user_id`,`cosmetic_id`),
  KEY `fk_inventory_cosmetic` (`cosmetic_id`),
  CONSTRAINT `fk_inventory_cosmetic` FOREIGN KEY (`cosmetic_id`) REFERENCES `cosmetics` (`cosmetic_id`) ON DELETE CASCADE,
  CONSTRAINT `fk_inventory_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventory`
--

LOCK TABLES `inventory` WRITE;
/*!40000 ALTER TABLE `inventory` DISABLE KEYS */;
/*!40000 ALTER TABLE `inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `leaderboard`
--

DROP TABLE IF EXISTS `leaderboard`;
/*!50001 DROP VIEW IF EXISTS `leaderboard`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `leaderboard` AS SELECT 
 1 AS `user_id`,
 1 AS `username`,
 1 AS `level`,
 1 AS `xp`,
 1 AS `proficiency`,
 1 AS `rank_position`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `player_stats`
--

DROP TABLE IF EXISTS `player_stats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `player_stats` (
  `stat_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `level` int NOT NULL DEFAULT '1',
  `xp` int NOT NULL DEFAULT '0',
  `proficiency` int NOT NULL DEFAULT '0',
  `last_played` date DEFAULT NULL,
  PRIMARY KEY (`stat_id`),
  KEY `fk_player_stats_user` (`user_id`),
  CONSTRAINT `fk_player_stats_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `player_stats`
--

LOCK TABLES `player_stats` WRITE;
/*!40000 ALTER TABLE `player_stats` DISABLE KEYS */;
INSERT INTO `player_stats` VALUES (9,17,2,50,15,NULL),(10,18,2,100,40,NULL),(11,19,3,180,51,NULL);
/*!40000 ALTER TABLE `player_stats` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_attempts`
--

DROP TABLE IF EXISTS `quest_attempts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quest_attempts` (
  `attempt_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `quest_id` int NOT NULL,
  `score` int NOT NULL DEFAULT '0',
  `completed` tinyint(1) NOT NULL DEFAULT '0',
  `xp_earned` int NOT NULL DEFAULT '0',
  `prof_change` int NOT NULL DEFAULT '0',
  `attempt_started` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `attempt_ended` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`attempt_id`),
  KEY `fk_attempts_user` (`user_id`),
  KEY `fk_attempts_quest` (`quest_id`),
  CONSTRAINT `fk_attempts_quest` FOREIGN KEY (`quest_id`) REFERENCES `quests` (`quest_id`) ON DELETE CASCADE,
  CONSTRAINT `fk_attempts_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_attempts`
--

LOCK TABLES `quest_attempts` WRITE;
/*!40000 ALTER TABLE `quest_attempts` DISABLE KEYS */;
/*!40000 ALTER TABLE `quest_attempts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questions`
--

DROP TABLE IF EXISTS `questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questions` (
  `question_id` int NOT NULL AUTO_INCREMENT,
  `quest_id` int NOT NULL,
  `question_text` text NOT NULL,
  `question_type` enum('multiple_choice','true_false','coding') NOT NULL,
  `correct_answer` text NOT NULL,
  `options` json DEFAULT NULL,
  PRIMARY KEY (`question_id`),
  KEY `fk_questions_quest` (`quest_id`),
  CONSTRAINT `fk_questions_quest` FOREIGN KEY (`quest_id`) REFERENCES `quests` (`quest_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questions`
--

LOCK TABLES `questions` WRITE;
/*!40000 ALTER TABLE `questions` DISABLE KEYS */;
/*!40000 ALTER TABLE `questions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quests`
--

DROP TABLE IF EXISTS `quests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quests` (
  `quest_id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(100) NOT NULL,
  `difficulty` enum('easy','medium','hard') NOT NULL DEFAULT 'easy',
  `xp_reward` int NOT NULL DEFAULT '10',
  `proficiency_gain` int NOT NULL DEFAULT '0',
  `description` text,
  PRIMARY KEY (`quest_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quests`
--

LOCK TABLES `quests` WRITE;
/*!40000 ALTER TABLE `quests` DISABLE KEYS */;
/*!40000 ALTER TABLE `quests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username` (`username`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (12,'Alice','alice@example.com','$2b$12$/xlkVLPtyDlHD36ax1LgJuwyJ1Po3YI3HqCvkRi97gjEHfAgEfehm','2025-10-21 23:14:07'),(13,'lj','ljrowh@galloway.com','$2b$12$vppiytFQG6AVfJx5yVsdUOto3JdGLj18aazlAUJIXxmqwxJHZTDUK','2025-10-21 23:19:37'),(14,'string','user@example.com','$2b$12$w67FJwLvkS3faO3Ds8x6su6wKa2a5aH9a0HRv/vd6kqZbu2u13SMG','2025-10-21 23:30:01'),(15,'Bob','bob@example.com','$2b$12$0nwU4mTLmrQlfQ1Sqg82CusoCyJbOl7RwBm0ReruuQCqLCygvEG.y','2025-10-22 01:35:39'),(16,'Charlie','charlie@example.com','$2b$12$J8ed5y8x.J/CLFCk.BebrOBHYE7n2GXeNcpUcwJ6k3gOciIZUwIT.','2025-10-22 01:36:29'),(17,'AutoStatTest','autostat@example.com','$2b$12$iYg0kmbGslE5L42hQdrAIeDRzN8CJPsAzbAtaJFATDjSPDrCqmCSy',NULL),(18,'will','will@example.com','$2b$12$raQSwOzn.u8IQgeY9nco2OW/sJ2XcGWkJqFtUfFe8bBxqt.n7RkW2',NULL),(19,'jeremy','jeremy@example.com','$2b$12$u7U5PXnJO3yQfpB2JUFrMuDFo29HcApBvNUkFMrifdhdSrEoInb3e',NULL);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Final view structure for view `leaderboard`
--

/*!50001 DROP VIEW IF EXISTS `leaderboard`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `leaderboard` AS select `u`.`user_id` AS `user_id`,`u`.`username` AS `username`,`ps`.`level` AS `level`,`ps`.`xp` AS `xp`,`ps`.`proficiency` AS `proficiency`,dense_rank() OVER (ORDER BY `ps`.`level` desc,`ps`.`xp` desc )  AS `rank_position` from (`users` `u` join `player_stats` `ps` on((`ps`.`user_id` = `u`.`user_id`))) */;
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

-- Dump completed on 2025-10-22 23:47:20
