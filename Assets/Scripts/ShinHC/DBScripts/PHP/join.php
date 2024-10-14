<?php 
$servername = "localhost:3306";
$username = "root";
$password = "";
$dbname = "db_login";

$joinID = $_POST["joinID"];
$joinPW = $_POST["joinPW"];
$joinName = $_POST["joinName"];
$joinGender = $_POST["joinGender"];
$joinTel = $_POST["joinTel"];


$conn = new mysqli($servername, $username, $password, $dbname);


$sql = "INSERT INTO `tb_user`(`id`, `pw`, `name`, `gender`, `tel`)
VALUES ('".$joinID."','".$joinPW."','".$joinName."','".$joinGender."','".$joinTel."')";
$conn->query($sql);


$conn->close();
?>