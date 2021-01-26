import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/employer_reviews.dart';
import 'package:housem8_flutter/services/employer_review_service.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';
import 'package:smooth_star_rating/smooth_star_rating.dart';

class EmployerReviewPage extends StatefulWidget {
  final int employerId;

  const EmployerReviewPage({Key key, this.employerId}) : super(key: key);

  @override
  _EmployerReviewScreen createState() => _EmployerReviewScreen();
}

class _EmployerReviewScreen extends State<EmployerReviewPage> {
  double _rating = 0.0;

  Widget ratingWidget() {
    return Container(
      padding: EdgeInsets.fromLTRB(16.0, 20.0, 0.0, 0.0),
      child: SmoothStarRating(
        rating: _rating,
        isReadOnly: false,
        size: 30,
        color: Colors.blue,
        borderColor: Colors.blue,
        filledIconData: Icons.star,
        halfFilledIconData: Icons.star_half,
        defaultIconData: Icons.star_border,
        starCount: 5,
        allowHalfRating: true,
        spacing: 2.0,
        onRated: (newRating) {
          _rating = newRating;
        },
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Pontuação ao cliente")),
      body: Container(
        margin: EdgeInsets.all(20),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Container(
              padding: EdgeInsets.fromLTRB(16.0, 20.0, 0.0, 0.0),
              child: Text('Avalie a sua experiência com o cliente',
                  style: TextStyle(
                      fontSize: 20.0,
                      fontWeight: FontWeight.bold,
                      color: Color(0xFF5B82AA))),
            ),
            ratingWidget(),
            SizedBox(height: 50),
            RaisedButton(
              padding: EdgeInsets.all(15.0),
              shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(30.0)),
              color: Colors.white,
              child: Text(
                'Avaliar',
                style: TextStyle(color: Colors.blue, fontSize: 16),
              ),
              onPressed: () async {
                await actionReviewEmployer();
              },
              //Send to API
            )
          ],
        ),
      ),
    );
  }

  // passar os dados
  Future<void> actionReviewEmployer() async {
    if (await ConnectionHelper.checkConnection()) {
      EmployerReviews newRating = new EmployerReviews(rating: _rating);

      // mudar para id do employer
      var statusCode = await EmployerReviewService()
          .createEmployerReview(widget.employerId, newRating);

      if (statusCode == 200) {
        Navigator.pop(context);
        Fluttertoast.showToast(
            msg: "Obrigado pela sua avaliação!",
            toastLength: Toast.LENGTH_LONG,
            gravity: ToastGravity.BOTTOM,
            timeInSecForIosWeb: 5,
            backgroundColor: Colors.lightBlue,
            textColor: Colors.white,
            fontSize: 16.0);
      }
    } else {
      showDialog(
        context: context,
        builder: (context) => ErrorMessageDialog(
            title: "Sem conexão",
            text: "Dispositivo não consegue conectar ao servidor!"),
      );
    }
  }
}
