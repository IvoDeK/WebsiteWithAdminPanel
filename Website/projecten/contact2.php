<?php
session_start();

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "projecten";


$Naam = $_GET['DataNaam'];
$Vraag = $_GET['tbVraag'];
$Telefoon = $_GET['DataTelefoon'];
$Email = $_GET['DataEmail'];

$GetPartijId = $_GET["SelectPartij"];

// Maak connectie
$conn = mysqli_connect("46.249.204.191", "ilybotc1_admin", "SKa~7#RXwgJ%zW*ou", "ilybotc1_projecten");

// Controleer connectie
if ($conn->connect_error) {
    die("Connectie -mislukt: " . $conn->connect_error);
}

if (isset($_GET['btnContact'])) {

    $sql = "INSERT INTO contact (partijid, contactnaam, contactmail, contactnummer, contactvraag)
    VALUES ('$GetPartijId', '$Naam', '$Email', '$Telefoon', '$Vraag')";
    $result = $conn->query($sql);
  }
  else {
    echo "Er is iets mis gegaan";
  }

$conn->close();
?>
<!DOCTYPE html>
<html lang="en" dir="ltr">
  <head>
    <meta charset="utf-8">
    <link rel="stylesheet" href="style.css">
    <title></title>
  </head>
  <body>
    <header>
    <div class="container">
      <div id="logo">
        <h1>Staatsverkiezingen</h1>
      </div>
        <nav>
          <ul>
            <li><a href="home.html">Home</a></li>
            <li><a href="verkiezingen.html">Verkiezingen</a></li>
            <li><a href="partij.php">Partijen</a></li>
            <li><a href="over.html">Over</a></li>
            <li class="current"><a href="contact.php">Contact</a></li>
          </ul>
        </nav>
      </div>
    </header>

    <div class="dark">
      <?php
       $e = "Beste, $Naam ($Telefoon) ($Email)<br/>";
       $e.= "Hartelijk dank voor uw vraag <br/>";
       $e.= "$Vraag <br/> ";
       $e.= "<br/>";
       $e.= "Uw vraag is verzonden en krijgt zo spoedig mogelijk bericht terug.<br/> ";
       $e.= "Staatsverkiezingen.<br/>";
       echo $e;

      ?>
    </div>

    <footer>
      <p>Staatsverkiezingen, Copyright &copy; 2019</p>
    </footer>
  </body>
  </body>
</html>
