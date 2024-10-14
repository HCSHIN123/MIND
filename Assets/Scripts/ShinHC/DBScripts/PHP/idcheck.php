<?php 
$servername = "localhost:3306";
$username = "root";
$password = "";
$dbname = "db_login";

$loginUser = $_POST["loginUser"];


$conn = new mysqli($servername, $username, $password, $dbname);


$sql = "SELECT * FROM tb_user WHERE exists (SELECT id FROM tb_user WHERE id =  '" .$loginUser. "');";
$result = $conn->query($sql);

if($result->num_rows > 0) {
	echo "invalid";
	exit;
} else {
	echo "valid";
	exit;
}

$conn->close();
?>

