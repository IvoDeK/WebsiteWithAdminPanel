<?php
  session_start();
  $connect = mysqli_connect("46.249.204.191", "ilybotc1_admin", "SKa~7#RXwgJ%zW*ou", "ilybotc1_projecten");

 ?>

<!DOCTYPE html>
<html lang="nl">
  <head>
    <meta charset="utf-8">
    <link rel="stylesheet" href="style.css">
    <title>Staatsverkiezingen | Contact</title>
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
            <li class="current"><a href="contact.html">Contact</a></li>
          </ul>
        </nav>
      </div>
    </header>

    <div class="fieldset">
      <form action="contact2.php" method="get">
        <p>Naam:</p>
        <input type="text" name="DataNaam" required>
        <p>Email:</p>
        <input type="text" name="DataEmail" required>
        <p>Telefoon:</p>
        <input type="text" name="DataTelefoon" required><br>
        <div class="styled-select blue semi-square">
         <select name="SelectPartij">
          <?php
          $query = "SELECT * FROM partijen ORDER BY partijid ASC";
          $result = mysqli_query($connect, $query);

          if(mysqli_num_rows($result) > 0)
          {
            while($row = mysqli_fetch_array($result))
            {
              $partijnaam = $row["partijnaam"];
              $partijid = $row["partijid"];
          ?>
            <option value="<?php echo $partijid ?>"><?php echo $partijnaam ?></option>
            <?php
            }
          }
          ?>
        </select>
        </div>
        <p>Vraag:</p>
        <textarea class="vraagText" name="tbVraag" required></textarea><br>
        <input type="submit" name="btnContact" value="Submit">
      </form>
    </div>
    <footer>
      <p>Staatsverkiezingen, Copyright &copy; 2019</p>
    </footer>
  </body>
</html>