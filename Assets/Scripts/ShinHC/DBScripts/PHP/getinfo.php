<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "db_login";

$loginUser = $_POST["loginUser"];

$conn = new mysqli($servername, $username, $password, $dbname);

if($conn->connect_error) {
	die("connection failed: " . $conn->connect_error);
}

$sql = "SELECT `id`, `pw`, `name`, `gender`, `tel` FROM `tb_user` WHERE id = '" .$loginUser. "'";
$result = $conn->query($sql);

if($result->num_rows > 0) {
	
	while($row = $result->fetch_assoc())
	{
		echo "id: " .$row['id']. " \npw: " .$row['pw']. "\nname: ".$row['name']."\ngender: ".$row['gender']."\nphonnumber: ".$row['tel']."\n";
	}
	
}

$conn->close();

?>