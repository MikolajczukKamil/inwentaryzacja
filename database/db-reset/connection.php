<?php

function getConnection() {
  return new mysqli("localhost","root","", "aplikacja_do_inwentaryzacji");
}
