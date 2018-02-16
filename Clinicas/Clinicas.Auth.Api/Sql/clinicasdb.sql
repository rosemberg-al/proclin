-- phpMyAdmin SQL Dump
-- version 4.6.5.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: 14-Abr-2017 às 21:29
-- Versão do servidor: 10.1.21-MariaDB
-- PHP Version: 7.1.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `clinicasdb`
--

-- --------------------------------------------------------

--
-- Estrutura da tabela `department`
--

CREATE TABLE `department` (
  `Id` int(10) NOT NULL,
  `Name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `feature`
--

CREATE TABLE `feature` (
  `Id` int(10) NOT NULL,
  `IdModule` int(10) NOT NULL,
  `Name` varchar(150) NOT NULL,
  `Controller` varchar(50) NOT NULL DEFAULT '',
  `Link` varchar(150) DEFAULT NULL,
  `Icone` varchar(30) DEFAULT NULL,
  `IdFeaturePai` int(10) DEFAULT NULL,
  `Situacao` varchar(1) DEFAULT NULL,
  `Ordenacao` int(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `featureaction`
--

CREATE TABLE `featureaction` (
  `Id` int(10) NOT NULL,
  `IdFeature` int(10) NOT NULL,
  `Action` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `module`
--

CREATE TABLE `module` (
  `Id` int(10) NOT NULL,
  `Name` varchar(40) NOT NULL,
  `KeyModule` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `role`
--

CREATE TABLE `role` (
  `Id` int(10) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `IdDepartment` int(10) NOT NULL,
  `Identifier` varchar(100) DEFAULT NULL,
  `IdFeatureInicial` int(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `rolefeature`
--

CREATE TABLE `rolefeature` (
  `Role_Id` int(10) NOT NULL,
  `Feature_Id` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `user`
--

CREATE TABLE `user` (
  `Id` int(10) NOT NULL,
  `UserName` varchar(25) NOT NULL,
  `Password` varchar(128) NOT NULL,
  `Name` varchar(120) NOT NULL,
  `Mobile` varchar(11) DEFAULT NULL,
  `Email` varchar(40) NOT NULL,
  `Status` int(10) NOT NULL,
  `Level` int(10) NOT NULL DEFAULT '0',
  `Token` varchar(200) DEFAULT NULL,
  `TokenExpiration` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `user`
--

INSERT INTO `user` (`Id`, `UserName`, `Password`, `Name`, `Mobile`, `Email`, `Status`, `Level`, `Token`, `TokenExpiration`) VALUES
(1, 'clinicas', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'Usuário Teste', '31999999999', 'clinicas@gmail.com', 1, 5, NULL, NULL);

-- --------------------------------------------------------

--
-- Estrutura da tabela `userfeature`
--

CREATE TABLE `userfeature` (
  `UserId` int(10) NOT NULL,
  `FeatureId` int(10) NOT NULL,
  `CanAccess` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `userfeatureaction`
--

CREATE TABLE `userfeatureaction` (
  `FeatureActionId` int(10) NOT NULL,
  `UserId` int(10) NOT NULL,
  `FeatureId` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `userrole`
--

CREATE TABLE `userrole` (
  `User_Id` int(10) NOT NULL,
  `Role_Id` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `usertoken`
--

CREATE TABLE `usertoken` (
  `Id` int(10) NOT NULL,
  `AccessToken` varchar(2000) NOT NULL,
  `UserName` varchar(25) NOT NULL,
  `Expiration` datetime NOT NULL,
  `Revoke` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `usertoken`
--

INSERT INTO `usertoken` (`Id`, `AccessToken`, `UserName`, `Expiration`, `Revoke`) VALUES
(1, 'ZRMScWAEjzPbOHRweZRGwGcXb_2M4Eu4rKDdTIaKmLZbtiL4RbiV1IUs-FZwsH_rxJEN498t1xYg0WpXQfYrN0AKRyWfFs7rjwnj13u6kyNsQbmYJ1bE9eYhfG1wTAqxh6I0odC1POkwFdVe6mraUVwHRBBmwoPo1RY-PZt57iPqXIpbWU0iGKHwM-rYoeMHdBOjafcSAK_wvWnpOEsGfTJMAm2gmrfjOmCtP2E62KEw_hnbBLihSQIXvn8cAfRfIup9Ng', 'clinicas', '2017-04-15 10:56:24', 0),
(2, 'bJFiUIVUDXIb_gmjm6U_Tq2KZicbDzo9Dhsiu24pEnynBHW8OIAO4QZSMb4024txNUeog57a_sHt0QlgDFVk8tC5HZoFyDXI9PrCKXY8j10Owg7ddl4-h8Y0oZWmoyTsXJj-WrX6xwL9HL7--H0Y7-jBTnF9sOmH4rQ-RSDJpIrV3X_4hV94gbNOeJTJhRyf570_1Jo3z1FtdhDCw1SV4jZdHYLWZoNaz_jcQtgqXEVg28UeP5XWAO9haMhNQIvaW4O8Ow', 'clinicas', '2017-04-15 11:01:16', 0),
(3, 'CN3h3xd-hgUdWaklBATmIBeUSDJ-Qj2nVpGW5-UDy8An6QdmwbhPvaQ1pkS2qqdATq-wNMK5Qyi7UL-wZNV0elCGA7Ta7-Nm5WW1B_5x16bmtx0yx1MdZqqQKWtoKw9ymdFt3oYEoMqWAxn4U0Fo0OswzjIt5w0aeW_xNTal8VIGm_cfCEKw6CQN-S43QiR23JEO18ml-ZtWrXCtRZqVG0XTEBQqDjDuTixB8rJyO6Jg_prrUa98FQWcggKroOAwMbxHfQ', 'clinicas', '2017-04-15 11:29:56', 0),
(4, 'LSXa01naCk2aTOcBEuT7cOHCeT7iFZMD3tUn0_WW-_JXRrjbOZU9NK9_60ei3-F7NCA_3VzMQR3_CI4LB3qTnKJZxw_n1BmIsmCJBDujIQrosT6epRKXYYid5PKFqNh1iUi1-3Co_2nkg8-rB0wFr7AkGiwtJkBaYlZX-0k3UkC13NyLuJltcgDlrEfJbSD6RRF30OgHhZ0Z1fKNKT-oxmR9txzjsuJkUoh4JeHpqbllviV458Zvy1yRxlM42151f7soRA', 'clinicas', '2017-04-15 11:30:23', 0),
(5, 'BN7n_WfKT7YIjP0zgE3cEkyL98ffItL-L8MaSd4OVOElT4LIYdFPByt-gkcvcTn3mlTOaC6PVkpvaZBwJMocd62dE_yLufZfMfnxNXlq1syx6n6QaiaJhrsYjoXX_sFlUMiBUXMhBzkYY--qvObSr1GC773SmioVv4gpIRel_JWQDiksycfDXCoBrXv6BHX_MvtDpxuy62MfVvu7XMJE7x5AS5_67pGJ0hpZzII-I60LjPUvLIS-s2dLxmJ-ft7umyfuCw', 'clinicas', '2017-04-15 11:31:52', 0),
(6, 'Wv7Ej5ESVdEwJIvhz0PyuyJZnkm7lbBPGLldEeK2UbZ9chK5PCWyTfWNvXltOG5Fplwp56vv0PgFfMtELHTIvXYvpm2plJo2-m9Au9XwklUl0r7Uqm8m14VCUft8mGjX7NqWrrZZ1T1tQ7kMTDaO-5uhWTr7lWwERAeHIfYvWdRHdWgV0Xrz4hHnn8G2S242EM1FIb7g6SPIWRlnc_wDhqvezB0zZ3GpKyw4BNPxGWcQnStlXm6GMOq4IYhaipcM-JiKPQ', 'clinicas', '2017-04-15 11:33:43', 0),
(7, 'xq-YTxXmec9tBkhTzLz1pw1QQrTvWeBSeN4daPvrQRIfCFIxKEWktgd-osGyBNYgLX0dXcJAD_7IbZ6BSat0zn0mRvNYZmJr9ypZKgRFDgfYxTVMp4zlR8me1oNrVBf0KSTrVqcHXJZBz_xFOmvcUkDnjJ_bOYIsJkUUdSyAEKyzvDQbeRSSBl_SoAC0g0GQSD64w9bWBvybYF5WgkloEnjN8eHlwo-1IdTOOd2cpvfGgoUBe_nRQ-yj4tZjaN_UWDNe_qrKK97ezzCwFIBYTHAnHcQ', 'clinicas', '2017-04-15 11:34:16', 0),
(8, 'LAs6k4tlKGVZX0qtQFANQ06WfeLkk2Qzkh7dcSxAAude30N-8sWBNTkY-prsbhiFmFcgy72_eroFKWtZcChP3TBql4OrWpLXUJTay0V-1HgtC9u5Uxv8PbOqhe52OwORY9pG8mQyxU3RHSx2DoxWzjZ-YVJlQ2d3bb2OJCBHbjNqROrFNlrV-65J4BedAiOu-a9jukjGkv0EE8WAfCC-st6Yyle-bl3PxBftcNFdyu9_js51is2uPKin0XYCecO5eGwczoM3KRtloPL7QNK31zknuV0', 'clinicas', '2017-04-15 11:35:22', 0),
(9, '2MRXZH9jceW1QjVhYCLvsssZmNB5mLNs-B5f8GdiziDs6jSN6Mh-xJ2ZZuWc-qHLvDRewzsglSheipVATuP6sz4V0iwMF4N6y-2R0g1PzMYlxTf6khqvN_XX78JOxAbcjgPSar3eVzSAWOfLzY2sSXuRnG7PD_3RlJIAfSqbgUUtCZH1UIShQZd1pbzQieh3_w397DIK1JYOAg4V-e8XA6t6wAHcmTS5Va41pMNc0SrJbKuvPfok9-xvTAla76BWlhU302QCuMav4UOJsq4IZF_RI_w', 'clinicas', '2017-04-15 16:20:18', 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `department`
--
ALTER TABLE `department`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `feature`
--
ALTER TABLE `feature`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_feature_module` (`IdModule`);

--
-- Indexes for table `featureaction`
--
ALTER TABLE `featureaction`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_feature_action` (`IdFeature`);

--
-- Indexes for table `module`
--
ALTER TABLE `module`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `role`
--
ALTER TABLE `role`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_role_department` (`IdDepartment`);

--
-- Indexes for table `rolefeature`
--
ALTER TABLE `rolefeature`
  ADD KEY `fk_role_feature` (`Role_Id`),
  ADD KEY `fk_feature_role` (`Feature_Id`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `userfeature`
--
ALTER TABLE `userfeature`
  ADD KEY `fk_feature` (`UserId`),
  ADD KEY `fk_feature_user` (`FeatureId`);

--
-- Indexes for table `userfeatureaction`
--
ALTER TABLE `userfeatureaction`
  ADD PRIMARY KEY (`FeatureActionId`),
  ADD KEY `fk_user_feature_action` (`UserId`),
  ADD KEY `fk_user_feature` (`FeatureId`);

--
-- Indexes for table `userrole`
--
ALTER TABLE `userrole`
  ADD KEY `fk_user_role` (`User_Id`),
  ADD KEY `fk_role_user` (`Role_Id`);

--
-- Indexes for table `usertoken`
--
ALTER TABLE `usertoken`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `department`
--
ALTER TABLE `department`
  MODIFY `Id` int(10) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `feature`
--
ALTER TABLE `feature`
  MODIFY `Id` int(10) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `featureaction`
--
ALTER TABLE `featureaction`
  MODIFY `Id` int(10) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `module`
--
ALTER TABLE `module`
  MODIFY `Id` int(10) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `role`
--
ALTER TABLE `role`
  MODIFY `Id` int(10) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `Id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `userfeatureaction`
--
ALTER TABLE `userfeatureaction`
  MODIFY `FeatureActionId` int(10) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `usertoken`
--
ALTER TABLE `usertoken`
  MODIFY `Id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
--
-- Constraints for dumped tables
--

--
-- Limitadores para a tabela `feature`
--
ALTER TABLE `feature`
  ADD CONSTRAINT `fk_feature_module` FOREIGN KEY (`IdModule`) REFERENCES `module` (`Id`);

--
-- Limitadores para a tabela `featureaction`
--
ALTER TABLE `featureaction`
  ADD CONSTRAINT `fk_feature_action` FOREIGN KEY (`IdFeature`) REFERENCES `feature` (`Id`);

--
-- Limitadores para a tabela `role`
--
ALTER TABLE `role`
  ADD CONSTRAINT `fk_role_department` FOREIGN KEY (`IdDepartment`) REFERENCES `department` (`Id`);

--
-- Limitadores para a tabela `rolefeature`
--
ALTER TABLE `rolefeature`
  ADD CONSTRAINT `fk_feature_role` FOREIGN KEY (`Feature_Id`) REFERENCES `feature` (`Id`),
  ADD CONSTRAINT `fk_role_feature` FOREIGN KEY (`Role_Id`) REFERENCES `role` (`Id`);

--
-- Limitadores para a tabela `userfeature`
--
ALTER TABLE `userfeature`
  ADD CONSTRAINT `fk_feature` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`),
  ADD CONSTRAINT `fk_feature_user` FOREIGN KEY (`FeatureId`) REFERENCES `feature` (`Id`);

--
-- Limitadores para a tabela `userfeatureaction`
--
ALTER TABLE `userfeatureaction`
  ADD CONSTRAINT `fk_user_feature` FOREIGN KEY (`FeatureId`) REFERENCES `feature` (`Id`),
  ADD CONSTRAINT `fk_user_feature_action` FOREIGN KEY (`UserId`) REFERENCES `role` (`Id`);

--
-- Limitadores para a tabela `userrole`
--
ALTER TABLE `userrole`
  ADD CONSTRAINT `fk_role_user` FOREIGN KEY (`Role_Id`) REFERENCES `role` (`Id`),
  ADD CONSTRAINT `fk_user_role` FOREIGN KEY (`User_Id`) REFERENCES `user` (`Id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
