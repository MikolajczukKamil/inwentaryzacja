<!doctype html>
<html>
	<head>
		<meta charset="UTF-8" />
		<title>Reset bazy do inwentaryzacji</title>
		<link rel="Stylesheet" href="./style.css" type="text/css">
	</head>
	<body>
		<div id="kontener">
			<h1>Reset bazy danych</h1>
			<form id="logowanie" action="./index.php" method="post">
				<p><input type="password" id="password" name="password" placeholder="Wpisz hasło..."/></p>
				<p><button class="przycisk" type="submit" name="akcja" value="p1">Resetuj bazę danych</button></p>
				<p><button class="przycisk" type="submit" name="akcja" value="p2">Resetuj procedury</button></p>
				<p><button class="przycisk" type="submit" name="akcja" value="p3">Resetuj bazę danych oraz procedury</button></p>
			</form>
			<?php
				require_once './connection.php';

				error_reporting(0);
			
				if(isset($_POST['akcja']) && isset($_POST['password']))
				{
					error_reporting(0);
					$password = "ResetBazy102938";
					
					if($_POST['password']===$password)
					{
						$conn = getConnection();

						if($conn->connect_error)
						{
							echo '<p class="komunikatBad">Nie udało się połączyć z bazą danych</p>';
							die();
						}
						
						$url = 'https://raw.githubusercontent.com/MikolajczukKamil/inwentaryzacja/database/database/dist/';
						
						if($_POST['akcja']==="p1")
						{
							$f = 'db-data.sql';
						}
						else if($_POST['akcja']==="p2")
						{
							$f = 'db-procedures.sql';
						}
						else if($_POST['akcja']==="p3")
						{
							$f = 'db-full.sql';
						}

						$content = file_get_contents($url . $f);
						
						
						if($content)
						{
							if($conn->multi_query($content) === TRUE)
							{
								echo '<p class="komunikatGood">Wykonano reset bazy danych!</p>';
							}
							else
							{
								echo '<p class="komunikatBad">Nie udało się zresetować bazy danych.</p>';
								//echo '<p>' . $conn->error . '</p>';
							}
						}
						else
						{
							echo '<p class="komunikatBad">Nie udało się pobrać zapytania do resetu bazy.</p>';
						}
					}
					else
					{
						echo '<p class="komunikatBad">Złe hasło.</p>';
					}
				}
			?>
		</div>
	</body>
</html>
