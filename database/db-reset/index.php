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
				<p><input type="password" id="password" name="password" placeholder="Wpisz hasło..." value="<?php if(isset($_POST['password'])) echo $_POST['password']; ?>"/></p>
				<p><button class="przycisk" type="submit" name="akcja" value="p1">Resetuj dane</button></p>
				<p><button class="przycisk" type="submit" name="akcja" value="p2">Resetuj procedury</button></p>
				<p><button class="przycisk" type="submit" name="akcja" value="p3">Resetuj funkcje</button></p>
				<p><button class="przycisk" type="submit" name="akcja" value="p4">Resetuj wszystko</button></p>
			</form>
			<?php
				function getContent($url){
					$ch = curl_init();
					curl_setopt($ch, CURLOPT_URL, $url);
					curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
					curl_setopt($ch, CURLOPT_USERAGENT, 'Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.13) Gecko/20080311 Firefox/2.0.0.13');
					$str = curl_exec($ch);
					curl_close($ch);
					return $str;
				}
				
				require_once './connection.php';

				error_reporting(0);
			
				if(isset($_POST['akcja']) && isset($_POST['password']))
				{
					error_reporting(0);
					$passwords = ["ResetBazy102938", "reset"];
					
					if(in_array($_POST['password'], $passwords))
					{
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
							$f = 'db-functions.sql';
						}
						else if($_POST['akcja']==="p4")
						{
							$f = 'db-full.sql';
						}
						else {
							echo '<p class="komunikatBad">Akcja nie znana</p>';
							die();
						}

						$conn = getConnection();

						if($conn->connect_error)
						{
							echo '<p class="komunikatBad">Nie udało się połączyć z bazą danych</p>';
							die();
						}

						$content = getContent($url . $f);
						
						if($content)
						{
							if($conn->multi_query($content))
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
