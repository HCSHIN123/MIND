<?php
$servername = "localhost:3308";
$username = "root";
$password = "rlacksgh83";
$dbname = "db_login";

$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];

$conn = new mysqli($servername, $username, $password, $dbname);

if($conn->connect_error) {
	die("connection failed: " .$conn->connect_error);
}

$sql = "SELECT * FROM tb_login WHERE id = '" .$loginUser. "'";
$result = $conn->query($sql);

if($result->num_rows > 0) {
	while($row = $result->fetch_assoc()) {
		if($row["pw"] == $loginPass) {
			echo "Login success!!";

			$time_sql = "SELECT datetime('now', 'localtime')";
			$time_result = $conn->query($sql);
			$row = time_result;
			$conn->close();
			exit;
		}
	}
	echo "Wrong password..";
} else {
	echo "ID not found..";
}

$conn->close();
?>