-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Jul 24, 2021 at 08:44 AM
-- Server version: 10.4.17-MariaDB
-- PHP Version: 7.3.27

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `siguri`
--

-- --------------------------------------------------------

--
-- Table structure for table `shpenzimet`
--

CREATE TABLE `shpenzimet` (
  `id` int(11) NOT NULL,
  `fatura` varchar(20) NOT NULL,
  `viti` int(4) NOT NULL,
  `muaji` int(2) NOT NULL,
  `total` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `shpenzimet`
--

INSERT INTO `shpenzimet` (`id`, `fatura`, `viti`, `muaji`, `total`) VALUES
(1, 'test', 2122, 2, 21),
(2, 'm', 1222, 2, 2),
(3, 'testfatura', 2021, 7, 100),
(4, 'testtt', 2021, 7, 200),
(5, 'testttt12', 1, 1, 1),
(7, 'testnjomza1', 2021, 7, 900),
(8, 'autobusi', 2021, 7, 2),
(12, 'as', 2000, 2, 2),
(13, 'as', 2000, 2, 2),
(14, 'as', 2000, 2, 2),
(21, 'njomzatesttt', 2015, 2, 154),
(22, 'njomzatesttt', 2015, 2, 154),
(23, 'busi', 2021, 3, 340);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `firstname` varchar(25) NOT NULL,
  `lastname` varchar(25) NOT NULL,
  `email` text NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` text NOT NULL,
  `salt` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`firstname`, `lastname`, `email`, `username`, `password`, `salt`) VALUES
('njomza rexhepi', 'njomza rexhepi', 'njomza@gmail.com', 'njomza', 'njomza', 'salt'),
('Njomza', 'Njomza', 'njomza2@gmail.com', 'njomzaa', '1000:WXP1VRRLltCF5vMKQGBBiXPl5FTfPoyB:aNHFcJrUBveqT6QpIefgP0J7LR1nor9K', 'salt'),
('Njomza Rexhepi', 'Njomza Rexhepi', 'njomzarexhepi@gmail.com', 'njomzarexhepi', '1000:gNVvqMx/cuYj9SPKX//EpNgu7MLFWsSf:7fUwOYp12iroMSZx37UwGCPWypFEH5Hz', 'salt');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `shpenzimet`
--
ALTER TABLE `shpenzimet`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `shpenzimet`
--
ALTER TABLE `shpenzimet`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
