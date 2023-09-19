-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 19-09-2023 a las 21:26:44
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `imortega`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `id` int(11) NOT NULL,
  `inmueble_id` int(11) DEFAULT NULL,
  `inquilino_id` int(11) DEFAULT NULL,
  `fecha_inicio` datetime DEFAULT NULL,
  `fecha_fin` datetime DEFAULT NULL,
  `monto_alquiler` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id`, `inmueble_id`, `inquilino_id`, `fecha_inicio`, `fecha_fin`, `monto_alquiler`) VALUES
(3, 1, 1, '2023-08-15 09:01:53', '2023-10-31 09:01:53', 30000.00),
(8, 12, 3, '2023-09-20 23:39:00', '2023-09-22 23:39:00', 25000.00),
(18, 17, 9, '2023-09-01 15:06:00', '2023-10-31 15:06:00', 80000.00),
(20, 16, 7, '2023-11-01 16:14:00', '2023-12-01 16:15:00', 7888.00),
(21, 13, 8, '2023-10-01 16:23:00', '2024-02-01 16:23:00', 25000.00);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id` int(11) NOT NULL,
  `direccion` varchar(255) NOT NULL,
  `ambientes` int(11) NOT NULL,
  `latitud` decimal(10,2) NOT NULL,
  `longitud` decimal(10,2) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `superficie` decimal(10,2) NOT NULL,
  `estado` tinyint(4) NOT NULL,
  `propietario_id` int(11) NOT NULL,
  `uso` int(11) NOT NULL,
  `tipo` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `direccion`, `ambientes`, `latitud`, `longitud`, `precio`, `superficie`, `estado`, `propietario_id`, `uso`, `tipo`) VALUES
(1, 'serranias 5', 4, -63.56, -97.23, 100000.00, 180.00, 1, 8, 1, 1),
(2, 'serranias 6', 4, -63.56, -97.23, 100000.00, 180.00, 0, 8, 1, 1),
(11, 'san martin 123', 3, -6456.00, -9723.00, 7690.00, 160.00, 1, 8, 1, 1),
(12, 'santos ortiz 3344', 2, -6356.00, -9823.00, 14000.00, 40.00, 1, 8, 1, 1),
(13, 'Junín 760', 3, -6456.00, -6687.00, 30000.00, 40.00, 1, 10, 1, 2),
(14, 'mitre 1340', 2, -6456.00, -9723.00, 40000.00, 40.00, 1, 11, 1, 2),
(15, 'Maipu 544', 4, -6456.00, -9723.00, 4000.00, 50.00, 1, 12, 2, 3),
(16, 'jujuy 6555', 3, -6456.00, -9823.00, 30000.00, 56.00, 1, 13, 1, 2),
(17, 'sucre 567', 5, -6456.00, -9723.00, 8000000.00, 70.00, 1, 15, 1, 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id` int(11) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `direccion` varchar(45) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `nacimiento` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id`, `dni`, `nombre`, `apellido`, `direccion`, `telefono`, `email`, `nacimiento`) VALUES
(1, '1234455', 'adolfo', 'hitler', 'av.muerte 456', '5656565', 'adolf@mail.com', '1957-02-17 00:00:00'),
(2, '2234455', 'marcos', 'peña', 'av.muerte 456', '5656565', 'ma@mail.com', '1957-02-17 11:23:40'),
(3, '78634455', 'mono', 'gatica', 'av.master 456', '34566565', 'mono@mail.com', '1980-02-17 11:23:40'),
(6, '34567899', 'Pablo', 'Perez', 'España 2341', '56779986', 'pablo@mail.com', '2014-01-29 00:00:00'),
(7, '2222222', 'Pedro', 'Suarez', 'los tamarindos 234', '26655555', 'suarez@mai.com', '2012-02-14 00:00:00'),
(8, '2576666', 'Mari', 'P', 'Colón 320', '266444444', 'mari@mail.com', '2014-02-05 00:00:00'),
(9, '678990', 'Jose', 'm', '3456sur', '78999000', 'j@mail.com', '2023-09-01 00:00:00');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `contrato_id` int(11) NOT NULL,
  `fecha_pago` datetime NOT NULL,
  `importe` decimal(10,2) DEFAULT NULL,
  `periodo` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `contrato_id`, `fecha_pago`, `importe`, `periodo`) VALUES
(1, 3, '2023-09-07 00:00:00', 2000.00, '2023-09-07 00:00:00'),
(2, 3, '2023-09-08 00:00:00', 2000.00, '2023-09-01 00:00:00'),
(3, 3, '2023-09-13 00:00:00', 2000.00, '2023-09-30 00:00:00'),
(4, 3, '2023-09-13 00:00:00', 3000.00, '2023-09-01 00:00:00'),
(6, 8, '2023-09-13 00:00:00', 25000.00, '2023-09-01 00:00:00'),
(13, 18, '2023-09-19 00:00:00', 8000.00, '2023-09-01 00:00:00'),
(14, 20, '2023-09-19 00:00:00', 2000.00, '2023-10-01 00:00:00');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id` int(11) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `direccion` varchar(45) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `nacimiento` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id`, `dni`, `nombre`, `apellido`, `direccion`, `telefono`, `email`, `nacimiento`) VALUES
(8, '2098765', 'Edmundo', 'Tremor', 'Junin 2765', '9998777', 'edmund@mail.com', '2015-01-05 00:00:00'),
(10, '34565657', 'Ariel', 'Mototo', 'Junin 2765', '34455667', 'ariel@mail.com', '2023-03-29 00:00:00'),
(11, '123456', 'Tomás', 'Perez', 'Mitre 1240', '26655786', 'tomy@mail.com', '1965-07-08 00:00:00'),
(12, '3455667', 'Mario', 'Sosa', 'Salta 234', '2666555444', 'mario@mail.com', '2005-02-09 00:00:00'),
(13, '2456533', 'Pedro', 'B', 'san luis123', '2222', 'pedro@mail.com', '2023-08-28 00:00:00'),
(15, '45555566', 'Pepe', 'camisa', 'ss456', '34556', 'camisa@mail.com', '2023-08-30 00:00:00');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `rol`
--

CREATE TABLE `rol` (
  `id` int(11) NOT NULL,
  `Nombre` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `rol`
--

INSERT INTO `rol` (`id`, `Nombre`) VALUES
(1, 'Administrador'),
(2, 'Empleado');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo`
--

CREATE TABLE `tipo` (
  `id` int(11) NOT NULL,
  `tipo` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo`
--

INSERT INTO `tipo` (`id`, `tipo`) VALUES
(1, 'Departamento'),
(2, 'Local'),
(3, 'Casa');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `uso`
--

CREATE TABLE `uso` (
  `id` int(11) NOT NULL,
  `uso` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `uso`
--

INSERT INTO `uso` (`id`, `uso`) VALUES
(1, 'Comercial'),
(2, 'Residencial');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `avatar` varchar(500) DEFAULT NULL,
  `clave` varchar(800) NOT NULL,
  `rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `nombre`, `apellido`, `email`, `avatar`, `clave`, `rol`) VALUES
(4, 'Basty', 'O', 'basty@mail.com', '/Uploads\\avatar_4.png', 'E3y6jCjTHIy8atfF3e/lKYzJiPWvLe7vNo5qXVtOfFs=', 1),
(17, 'Perla', 'P', 'perla@mail.com', '/Uploads\\avatar_17.jfif', '1xWPXpWVqeUJPsIjtUOLI8ECkOK5Nhcneq7UfRTcO/Y=', 2),
(19, 'Lucho', 'T', 'lucho@mail.com', '/Uploads\\avatar_19.jfif', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA=', 2);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id`),
  ADD KEY `contrato_ibfk_1` (`inmueble_id`),
  ADD KEY `contrato_ibfk_2` (`inquilino_id`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_tipo` (`tipo`),
  ADD KEY `fk_uso` (`uso`),
  ADD KEY `inmueble_ibfk_1` (`propietario_id`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD KEY `pago_ibfk_1` (`contrato_id`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `rol`
--
ALTER TABLE `rol`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `tipo`
--
ALTER TABLE `tipo`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `uso`
--
ALTER TABLE `uso`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `tipo`
--
ALTER TABLE `tipo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `uso`
--
ALTER TABLE `uso`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`inmueble_id`) REFERENCES `inmueble` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`inquilino_id`) REFERENCES `inquilino` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `fk_tipo` FOREIGN KEY (`tipo`) REFERENCES `tipo` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_uso` FOREIGN KEY (`uso`) REFERENCES `uso` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`propietario_id`) REFERENCES `propietario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`contrato_id`) REFERENCES `contrato` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
