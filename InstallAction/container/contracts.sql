-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 24-03-2022 a las 22:54:45
-- Versión del servidor: 10.4.22-MariaDB
-- Versión de PHP: 8.1.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `contracts`
--
CREATE DATABASE IF NOT EXISTS `contracts` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `contracts`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE IF NOT EXISTS `contrato` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `id_proveedor` int(10) NOT NULL,
  `id_dictamen` int(10) DEFAULT NULL,
  `numero` varchar(20) DEFAULT NULL,
  `duracion` int(10) DEFAULT NULL,
  `fecha_firma` date DEFAULT NULL,
  `fecha_venc` date DEFAULT NULL,
  `observaciones` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FKContrato555231` (`id_proveedor`),
  KEY `FKContrato803084` (`id_dictamen`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `dictamen`
--

CREATE TABLE IF NOT EXISTS `dictamen` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `numero` varchar(20) NOT NULL,
  `aprobado` tinyint(1) NOT NULL DEFAULT 0,
  `observaciones` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedor`
--

CREATE TABLE IF NOT EXISTS `proveedor` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(255) NOT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`),
  UNIQUE KEY `nombre` (`nombre`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `roles`
--

CREATE TABLE IF NOT EXISTS `roles` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `mostrar` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `roles`
--

INSERT IGNORE INTO `roles` (`id`, `name`, `mostrar`) VALUES
(1, 'ADMIN', 'Administrador'),
(2, 'USER', 'Usuario'),
(3, 'CONT', 'Planificador');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `users`
--

CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(64) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellidoP` varchar(50) NOT NULL,
  `apellidoM` varchar(50) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `enabled` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `users`
--

INSERT IGNORE INTO `users` (`id`, `username`, `password`, `nombre`, `apellidoP`, `apellidoM`, `email`, `telefono`, `enabled`) VALUES
(3, 'admin', '$2a$10$UZgor7s2J0d.n8d8.0Vl2.pQBsswH8I8bE9bPwJ9h.rUrClxO/FRa', 'Admin', 'Prueba', 'Numero', 'admin@admin.co', NULL, 1),
(6, 'danny98cuba', '$2a$10$WEGO0TNl1TQbqeq6wrSUvebDvDUUiVw8TH7atiUm6hd85vMb2EXia', 'Daniel', 'González', 'Cuétara', 'danny.glezcuet@gmail.com', '53741292', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `user_role`
--

CREATE TABLE IF NOT EXISTS `user_role` (
  `user_id` int(11) NOT NULL,
  `roles_id` int(11) NOT NULL,
  KEY `FK_USERS_ROLES` (`user_id`),
  KEY `FK_ROLES_USERS` (`roles_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `user_role`
--

INSERT IGNORE INTO `user_role` (`user_id`, `roles_id`) VALUES
(3, 1),
(6, 2),
(6, 3);

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `FKContrato555231` FOREIGN KEY (`id_proveedor`) REFERENCES `proveedor` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FKContrato803084` FOREIGN KEY (`id_dictamen`) REFERENCES `dictamen` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `user_role`
--
ALTER TABLE `user_role`
  ADD CONSTRAINT `FK_ROLES_USERS` FOREIGN KEY (`roles_id`) REFERENCES `roles` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_USERS_ROLES` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
