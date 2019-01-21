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
    $query = "SELECT userEmail FROM Users WHERE userEmail='$email'";
    $result = mysqli_query($dblink, $query);
    $row = mysqli_fetch_row($result);

    $errorMessage = '';
    $id = -1;

    $tf = false;

    if ($row)
    {
        $errorMessage = 'Invalid Registration: User already exists!';
    }
    else
    {
        $query2 = "INSERT INTO Users (userEmail, userPass) VALUES ('$email', '$encryptedPassword')";
        $result2 = mysqli_query($dblink, $query2);
        $row2 = mysqli_fetch_row($result2);

        if ($row2)
        {
            $errorMessage = 'Registration Failed';
        }
        else
        {
            $tf = true;
            
            $id = mysqli_insert_id($dblink);
        }
    }

    $dataArray = array('success' => $tf, 'error' => $errorMessage, 'email' => "$email", 'id' => $id);

    header('Content-Type: application/json');
    echo json_encode($dataArray);
?>