import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class CountingStars extends StatelessWidget {
  final double averageRating;
  final String role;

  CountingStars(
    this.averageRating,
    this.role, {
    Key key,
  });

  @override
  Widget build(BuildContext context) {
    if (this.role == "MATE") {
      return Row(
        mainAxisSize: MainAxisSize.min,
        children: (() {
          if (this.averageRating == null ||
              (this.averageRating == 0 && this.averageRating < 0.5)) {
            return <Widget>[
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
            ];
          } else if (this.averageRating >= 0.5 && this.averageRating < 1) {
            return <Widget>[
              Icon(Icons.star_half, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
            ];
          } else if (this.averageRating >= 1 && this.averageRating < 1.5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
            ];
          } else if (this.averageRating >= 1.5 && this.averageRating < 2) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star_half, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
            ];
          } else if (this.averageRating >= 2 && this.averageRating < 2.5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
            ];
          } else if (this.averageRating >= 2.5 && this.averageRating < 3) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star_half, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
            ];
          } else if (this.averageRating >= 3 && this.averageRating < 3.5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
            ];
          } else if (this.averageRating >= 3.5 && this.averageRating < 4) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star_half, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
            ];
          } else if (this.averageRating >= 4 && this.averageRating < 4.5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star_border, color: Color(0xFF1565C0)),
            ];
          } else if (this.averageRating >= 4.5 && this.averageRating < 5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star_half, color: Color(0xFF1565C0)),
            ];
          } else {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
              Icon(Icons.star, color: Color(0xFF1565C0)),
            ];
          }
        }()),
      );
    } else {
      return Row(
        mainAxisSize: MainAxisSize.min,
        children: (() {
          if (this.averageRating == null ||
              (this.averageRating == 0 && this.averageRating < 0.5)) {
            return <Widget>[
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
            ];
          } else if (this.averageRating >= 0.5 && this.averageRating < 1) {
            return <Widget>[
              Icon(Icons.star_half, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
            ];
          } else if (this.averageRating >= 1 && this.averageRating < 1.5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
            ];
          } else if (this.averageRating >= 1.5 && this.averageRating < 2) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star_half, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
            ];
          } else if (this.averageRating >= 2 && this.averageRating < 2.5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
            ];
          } else if (this.averageRating >= 2.5 && this.averageRating < 3) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star_half, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
            ];
          } else if (this.averageRating >= 3 && this.averageRating < 3.5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
            ];
          } else if (this.averageRating >= 3.5 && this.averageRating < 4) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star_half, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
            ];
          } else if (this.averageRating >= 4 && this.averageRating < 4.5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star_border, color: Color(0xFF006064)),
            ];
          } else if (this.averageRating >= 4.5 && this.averageRating < 5) {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star_half, color: Color(0xFF006064)),
            ];
          } else {
            return <Widget>[
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
              Icon(Icons.star, color: Color(0xFF006064)),
            ];
          }
        }()),
      );
    }
  }
}
