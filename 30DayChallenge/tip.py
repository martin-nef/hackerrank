def solution(meal_total, tip, tax):
	return round(meal_total + meal_total * (tip/100) + meal_total * (tax/100))
