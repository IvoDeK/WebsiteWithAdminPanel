<?php
    $connect = mysqli_connect("46.249.204.191", "ilybotc1_admin", "SKa~7#RXwgJ%zW*ou", "ilybotc1_projecten");
 ?>

<!DOCTYPE html>
<html lang="nl">
  <head>
    <meta charset="utf-8">
    <link rel="stylesheet" href="style.css">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <title>Staatsverkiezingen | Partij</title>
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
            <li class="current"><a href="partij.html">Partijen</a></li>
            <li><a href="over.html">Over</a></li>
            <li><a href="contact.php">Contact</a></li>
          </ul>
        </nav>
      </div>
    </header>

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
    <a href="partij.php?partijid=<?php echo "$partijid"; ?>">  <button class="btn info"><?php echo $partijnaam ?></button></a>&nbsp;
    <?php
      }
    }
    if (isset($_GET["partijid"]))
    {
      $partijid = $_GET["partijid"];
      $query = "SELECT * FROM partijen WHERE partijid = '$partijid'";
      $result = mysqli_query($connect, $query);
        if(mysqli_num_rows($result) > 0)
        {
          echo "<br/>";
          while($row = mysqli_fetch_array($result))
          {
            ?>
            <div class="col-sm-4 col-md-3">
                <div class="products" align="center">
                  <img class="img-style" src="data:image/jpeg;base64, <?php echo base64_encode($row['partijfoto'])?>"><br />
                  <h4 class="text-info"><?php echo $row["partijnaam"]; ?></h4>
                  <h4 class=""><?php echo $row["standpunten"]; ?></h4>
                  <h4 class=""><?php echo $row["partijinfo"]; ?></h4>
                </div>
              </form>
            </div>
        <?php
          }
        }
      }
      ?>
      </form>
      </div>
    </div>

    <section class="Contact">
      <div class="container">
        <h1>info@staatsverkiezingen.com</h1>
        <form>
          <h2>Klik <a href="contact.php">Hier</a> om contact op te nemen</h2>
        </form>
      </div>
    </section>

    <p>Hierboven staan alle partijen die momenteel mee doen met de verkiezingen. </p>
    <footer>
      <p>Staatsverkiezingen, Copyright &copy; 2019</p>
    </footer>
  </body>
</html>