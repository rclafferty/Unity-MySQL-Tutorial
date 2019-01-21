<?php
    include 'dbconnection.php';
    
    // Get the values from the post parameters
    $email = $_POST['email'];
    $upass = $_POST['password'];
    
    // Clean email/pass of SQL Injection tags
    $email = strip_tags($email);
    $upass = strip_tags($upass);

    // Encrypt password with SHA-256
    $encryptedPassword = hash('sha256', $upass);

    // See if user exists
    $query = "SELECT userID FROM Users WHERE userEmail='$email' and userPass='$encryptedPassword'";
    $result = mysqli_query($dblink, $query);
    $row = mysqli_fetch_row($result);

    $errorMessage = '';
    $id = -1;

    $tf = false;

    if ($row)
    {
        $tf = true;
        $id = $row[0];
    }
    else
    {
        $errorMessage = 'Invalid email or password';
    }
    
    $dataArray = array('success' => $tf, 'error' => $errorMessage, 'email' => "$email", 'id' => $id);

    header('Content-Type: application/json');
    echo json_encode($dataArray);
?>